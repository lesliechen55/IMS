using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Backend.Message.Model.Models;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;
using YQTrack.Core.Backend.Admin.Message.Core.Message;
using YQTrack.Core.Backend.Admin.Message.Data;
using YQTrack.Core.Backend.Admin.Message.Data.Models;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Standard.Common.Utils;

namespace YQTrack.Core.Backend.Admin.Message.Service.Imp
{
    public class SendTaskService : ISendTaskService
    {
        private readonly MessageDbContext _dbContext;
        private readonly UserDbContext _dbUserContext;
        //private readonly AdminDbContext _dbAdminContext;

        public SendTaskService(MessageDbContext dbContext, UserDbContext dbUserContext)//,AdminDbContext dbAdminContext
        {
            _dbContext = dbContext;
            _dbUserContext = dbUserContext;
            //_dbAdminContext = dbAdminContext;
        }

        /// <summary>
        /// 获取发送任务分页列表
        /// </summary>
        /// <param name="input">发送任务列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<SendTaskPageDataOutput> outputs, int total)> GetPageDataAsync(SendTaskPageDataInput input)
        {
            var queryable = (from sendTask in _dbContext.TsendTask
                             join template in _dbContext.TtemplateType
                             on sendTask.FtemplateTypeId equals template.FtemplateTypeId
                             into bb
                             from template in bb.DefaultIfEmpty()
                             join project in _dbContext.Tproject
                             on template.FprojectId equals project.FprojectId
                             into pp
                             from project in pp.DefaultIfEmpty()
                                 //join create in _dbAdminContext.Manager
                                 //on sendTask.FCreateBy equals create.FId
                                 //into mm
                                 //from create in mm.DefaultIfEmpty()
                                 //join update in _dbAdminContext.Manager
                                 //on sendTask.FUpdateBy equals update.FId
                                 //into uu
                                 //from update in uu.DefaultIfEmpty()
                                 //where (input.ProjectId.HasValue ? template.FprojectId == input.ProjectId.Value : true)
                                 //&& (input.ChannelId.HasValue ? template.Fchannel == input.ChannelId.Value : true)
                             select new SendTaskPageDataOutput
                             {
                                 FTaskId = sendTask.FtaskId,
                                 FChannel = template.Fchannel,
                                 FProjectId = project.FprojectId,
                                 FProjectName = project.FprojectName,
                                 FTemplateName = template.FtemplateName,
                                 FRemarks = sendTask.Fremarks,
                                 FState = sendTask.Fstate.HasValue ? sendTask.Fstate.Value : 0,
                                 FPushSucess = sendTask.FpushSucess.HasValue ? sendTask.FpushSucess.Value : 0,
                                 FPushFail = sendTask.FpushFail.HasValue ? sendTask.FpushFail.Value : 0,
                                 FCreateTime = sendTask.FcreateTime,
                                 FUpdateTime = sendTask.FupdateTime,
                                 FCreateBy = sendTask.FCreateBy,
                                 FUpdateBy = sendTask.FUpdateBy
                             });
            queryable = queryable.WhereIf(() => input.ProjectId.HasValue, x => x.FProjectId == input.ProjectId.Value);
            queryable = queryable.WhereIf(() => input.ChannelId.HasValue, x => x.FChannel == (int)input.ChannelId.Value);
            queryable = queryable.WhereIf(() => input.StartTime.HasValue, x => x.FUpdateTime.Value >= input.StartTime.Value.ToUniversalTime());
            queryable = queryable.WhereIf(() => input.EndTime.HasValue, x => x.FUpdateTime.Value <= input.EndTime.Value.AddDays(1).ToUniversalTime());
            queryable = queryable.WhereIf(() => !string.IsNullOrWhiteSpace(input.TemplateName), x => x.FTemplateName.Contains(input.TemplateName, StringComparison.InvariantCultureIgnoreCase));

            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FUpdateTime)
                .ToPage(input.Page, input.Limit)
                .ToListAsync();
            //outputs.ForEach(f =>
            //{
            //    if (f.FCreateBy != 0)
            //    {
            //        Manager create = _dbAdminContext.Manager.AsNoTracking().SingleOrDefault(s => s.FId == f.FCreateBy);
            //        f.CreateByName = create == null ? "" : create.FNickName;
            //    }
            //    if (f.FUpdateBy != 0)
            //    {
            //        Manager update = _dbAdminContext.Manager.AsNoTracking().SingleOrDefault(s => s.FId == f.FUpdateBy);
            //        f.UpdateByName = update == null ? "" : update.FNickName;
            //    }
            //});
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SendTaskEditOutput> GetByIdAsync(long id)
        {
            SendTaskEditOutput output = await _dbContext.TsendTask
                .Join(_dbContext.TsendTaskObj, x => x.FtaskId, y => y.FtaskId, (x, y) => new { SendTask = x, SendTaskObj = y })
                .Join(_dbContext.TtemplateType, x => x.SendTask.FtemplateTypeId, y => y.FtemplateTypeId, (x, y) => new { SendTask = x.SendTask, SendTaskObj = x.SendTaskObj, TemplateType = y })
                .Where(w => w.SendTask.FtaskId == id && w.SendTask.Fstate == (int)PushState.Draft)
                .Select(s => new SendTaskEditOutput
                {
                    TaskId = s.SendTask.FtaskId,
                    ObjType = s.SendTaskObj.FobjType,
                    Channel = (ChannelSend)s.TemplateType.Fchannel,
                    TemplateTypeId = s.TemplateType.FtemplateTypeId,
                    TemplateName = s.TemplateType.FtemplateName,
                    Remarks = s.SendTask.Fremarks,
                    ObjDetails = string.IsNullOrWhiteSpace(s.SendTaskObj.FobjDetails) ? "" : s.SendTaskObj.FobjDetails
                })
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (null == output)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(output)}数据不存在或状态已更新");
            }
            return output;
        }

        /// <summary>
        /// 添加发送任务
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorName">操作人</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(SendTaskEditInput input, string operatorName)
        {
            TtemplateType templateType = await ValidTemplateAsync(input);
            //添加发送任务
            TsendTask model = new TsendTask();//_mapper.Map<TsendTask>(input)
            model.FtaskId = IdHelper.GetGenerateId();
            model.FtemplateTypeId = input.TemplateTypeId;
            model.Fstate = input.SendAction == SendAction.Yes ? (int)PushState.ReadyPush : (int)PushState.Draft;
            model.FpushSucess = 0;
            model.FpushFail = 0;
            model.FretryCount = 0;
            model.Fremarks = input.Remarks;
            model.FpushTime = DateTime.UtcNow;
            model.FcreateTime = DateTime.UtcNow;
            model.FupdateTime = DateTime.UtcNow;
            model.FdataJson = templateType.FdataJson;
            model.FCreateBy = operatorName;
            model.FUpdateBy = operatorName;
            await _dbContext.TsendTask.AddAsync(model);

            TsendTaskObj sendTaskObj = new TsendTaskObj();
            sendTaskObj.FtaskObjId = IdHelper.GetGenerateId();
            sendTaskObj.FtaskId = model.FtaskId;
            var (objType, objDetail) = GetObjDataBySendType(input, templateType);
            sendTaskObj.FobjType = objType;
            sendTaskObj.FobjDetails = objDetail;
            await _dbContext.TsendTaskObj.AddAsync(sendTaskObj);

            return await _dbContext.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 验证并获取模板信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<TtemplateType> ValidTemplateAsync(SendTaskEditInput input)
        {
            TtemplateType templateType = await _dbContext.TtemplateType.AsNoTracking().SingleOrDefaultAsync(s => s.FtemplateTypeId == input.TemplateTypeId);
            if (templateType == null)
            {
                throw new BusinessException("手动发送任务异常，基础模板不存在");
            }
            if (!_dbContext.Ttemplate.Any(a => a.FtemplateTypeId == input.TemplateTypeId && a.FisDel == 0 && a.Flanguage == "en"))
            {
                throw new BusinessException("手动发送任务异常，英文模板不存在");
            }

            return templateType;
        }

        /// <summary>
        /// 根据发送类型获取发送数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="templateType"></param>
        private (ObjType objType, string objDetail) GetObjDataBySendType(SendTaskEditInput input, TtemplateType templateType)
        {
            ObjType objType = ObjType.None;
            string objDetail = string.Empty;
            //按角色发
            if (input.SendType == SendType.ByRole)
            {
                objType = ObjType.Role;
                objDetail = ((int)input.UserRoleType).ToString();
            }
            //按用户发
            else
            {
                objType = GetObjType((ChannelSend)templateType.Fchannel);
                objDetail = GetPushAddress(input.ObjDetails, objType);
            }
            return (objType, objDetail);
        }

        /// <summary>
        /// 获取对象类型
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private static ObjType GetObjType(ChannelSend channel)
        {
            ObjType objType = ObjType.None;
            switch (channel)
            {
                case ChannelSend.APP:
                    objType = ObjType.ChannelApp;
                    break;
                case ChannelSend.Email:
                    objType = ObjType.ChannelEmail;
                    break;
                case ChannelSend.SiteMessage:
                    objType = ObjType.ChannelSiteMessage;
                    break;
                default:
                    break;
            }
            if (objType == ObjType.None)
            {
                throw new BusinessException("手动发送任务异常，获取不到对象类型");
            }

            return objType;
        }

        /// <summary>
        /// 过滤需要发送的地址信息
        /// </summary>
        /// <param name="address"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private static string GetPushAddress(string address, ObjType types)
        {
            StringBuilder sb = new StringBuilder();
            address = address.Trim().Replace("\r\n", ",").Replace("\n", ",");
            string[] addresses = address.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in addresses)
            {
                switch (types)
                {
                    case ObjType.ChannelSiteMessage:
                        if (item.Trim().IsInt(false))
                        {
                            sb.Append($"{item},");
                        }
                        break;
                    case ObjType.ChannelEmail:
                        if (item.Trim().IsEmail(false))
                        {
                            sb.Append($"{item},");
                        }
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            sb.Append($"{item},");
                        }
                        break;
                }
            }
            if (sb.Length == 0)
            {
                throw new BusinessException("手动发送任务异常，没有正确的发送地址");
            }
            return sb.ToString().Substring(0, sb.Length - 1);
        }

        /// <summary>
        /// 修改发送任务
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorName">操作人</param>
        /// <returns></returns>
        public async Task<bool> EditAsync(SendTaskEditInput input, string operatorName)
        {
            //修改发送任务
            TsendTask model = await _dbContext.TsendTask.SingleOrDefaultAsync(s => s.FtaskId == input.TaskId && s.Fstate == (int)PushState.Draft);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.TaskId)}参数错误,{nameof(model)}数据不存在或状态已更新");
            }
            input.TemplateTypeId = model.FtemplateTypeId;
            TtemplateType templateType = await ValidTemplateAsync(input);

            model.Fstate = input.SendAction == SendAction.Yes ? (int)PushState.ReadyPush : (int)PushState.Draft;
            model.Fremarks = input.Remarks;
            model.FupdateTime = DateTime.UtcNow;
            model.FUpdateBy = operatorName;

            TsendTaskObj sendTaskObj = await _dbContext.TsendTaskObj.SingleOrDefaultAsync(s => s.FtaskId == input.TaskId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.TaskId)}参数错误,{nameof(model)}数据不存在");
            }
            var (objType, objDetail) = GetObjDataBySendType(input, templateType);
            sendTaskObj.FobjType = objType;
            sendTaskObj.FobjDetails = objDetail;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 测试发送语言模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SendTemplateTestAsync(SendTemplateTestInput input)
        {
            Ttemplate template = await _dbContext.Ttemplate.AsNoTracking().SingleOrDefaultAsync(s => s.FtemplateId == input.TemplateId);
            if (template == null)
            {
                throw new BusinessException("测试发送语言模板异常，语言模板不存在");
            }
            TtemplateType templateType = await _dbContext.TtemplateType.AsNoTracking().SingleOrDefaultAsync(s => s.FtemplateTypeId == template.FtemplateTypeId);
            if (templateType == null)
            {
                throw new BusinessException("测试发送语言模板异常，基础模板不存在");
            }

            MessageModel msg = null;
            if (templateType.FdataJson.IsNotNullOrWhiteSpace())
            {
                msg = SerializeExtend.MyDeserializeFromJson<MessageModel>(templateType.FdataJson);
            }
            if (msg == null)
            {
                msg = new MessageModel()
                {
                    MessageType = (MessageTemplateType)templateType.FtemplateCode,
                    TemplateData = "",
                    UserId = new UserInfoExt(),
                    SendTaskId = 0,
                    TransmitDataJson = ""
                };
            }
            if (msg.MessageType == MessageTemplateType.CommonTemplate)
            {
                //通用模版传递的是999999，所以接收端接收到之后，无法判断是哪个通用模版，所以需要将一些识别信息一起传递过去
                msg.TransmitDataJson = "FTemplateTypeId=" + templateType.FtemplateTypeId;
            }

            //添加发送任务
            TsendTask taskModel = new TsendTask();
            taskModel.FtaskId = IdHelper.GetGenerateId();
            taskModel.FtemplateTypeId = template.FtemplateTypeId;
            taskModel.FpushSucess = 0;
            taskModel.FpushFail = 0;
            taskModel.FretryCount = 0;
            taskModel.Fremarks = input.Remarks;
            taskModel.FpushTime = DateTime.UtcNow;
            taskModel.FcreateTime = DateTime.UtcNow;
            taskModel.FupdateTime = DateTime.UtcNow;
            taskModel.Fstate = (int)PushState.Pushing;
            taskModel.FdataJson = msg.TemplateData;

            TsendTaskObj sendTaskObj = new TsendTaskObj();
            sendTaskObj.FtaskObjId = IdHelper.GetGenerateId();
            sendTaskObj.FtaskId = taskModel.FtaskId;
            sendTaskObj.FobjType = GetObjType((ChannelSend)templateType.Fchannel);
            sendTaskObj.FobjDetails = GetPushAddress(input.ObjDetails, sendTaskObj.FobjType);

            msg.SendTaskId = taskModel.FtaskId;
            msg.UserId.FLanguage = template.Flanguage;

            if (TestSendMessage(templateType, sendTaskObj, msg, ref taskModel))
            {
                taskModel.Fstate = (int)PushState.PushFinish;
            }
            else
            {
                taskModel.Fstate = (int)PushState.PushError;
                taskModel.FretryCount++;
                taskModel.FpushSucess = 0;
                taskModel.FpushFail = 0;
            }
            await _dbContext.TsendTask.AddAsync(taskModel);
            await _dbContext.TsendTaskObj.AddAsync(sendTaskObj);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 直接调用消息模块
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="sendTaskObj"></param>
        /// <param name="msg"></param>
        /// <param name="sendTask"></param>
        /// <returns></returns>
        private bool TestSendMessage(TtemplateType templateType, TsendTaskObj sendTaskObj, MessageModel msg, ref TsendTask sendTask)
        {
            string[] addressList = sendTaskObj.FobjDetails.Split(',');
            switch ((ChannelSend)templateType.Fchannel)
            {
                case ChannelSend.APP:
                    foreach (string userToken in addressList)
                    {
                        msg.UserId.Token = userToken;
                        string pushProvider = _dbUserContext.TuserDevice.Where(w => w.FpushToken == userToken && w.FisPush.Value && w.FisValid.Value).Select(s => s.FpushProvider).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(pushProvider))
                        {
                            msg.UserId.PushProvider = pushProvider;
                            MessageHelper.SendMessage(msg);
                        }
                        else
                        {
                            sendTask.FpushFail++;
                        }
                    }
                    break;
                case ChannelSend.Email:
                    foreach (string email in addressList)
                    {
                        msg.UserId.FEmail = email;
                        MessageHelper.SendMessage(msg);
                    }
                    break;
                case ChannelSend.SiteMessage:
                    foreach (string userInfoId in addressList)
                    {
                        TsiteMessageDetails detail = new TsiteMessageDetails()
                        {
                            //FsiteMessageDetailsId = GenerateIdHelper.GetGenerateId(),
                            FsiteMessageId = 0,
                            FtemplateTypeId = templateType.FtemplateTypeId,
                            FcreateTime = DateTime.UtcNow,
                            Foverdue = DateTime.MaxValue,
                            FuserId = long.Parse(userInfoId),
                            FdataJson = sendTask.FdataJson,
                            FisRead = SiteMessageState.Unread,
                            FisDel = (int)Del.NoDel,
                            FupdateTime = DateTime.UtcNow
                        };
                        _dbContext.TsiteMessageDetails.Add(detail);
                        sendTask.FpushSucess++;
                    }
                    break;
            }
            return true;
        }
    }
}
