using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.Web
{
    public static class SeedData
    {
        public static void InitDbDefaultData(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var adminDbContext = services.GetRequiredService<AdminDbContext>();

#if DEBUG
                    // 调试模式下启动自动代码迁移数据库的方式
                    adminDbContext.Database.Migrate();
#endif

                    if (!adminDbContext.Permission.Any())
                    {
                        #region 初始化我的首页+初始化顶级菜单

                        var homeIndex = new Permission
                        {
                            FControllerName = "Home",
                            FActionName = "Index",
                            FFullName = "Home_Index",
                            FUrl = "/Home/Index",
                            FSort = 0,
                            FRemark = "我的首页",
                            FName = "我的首页",
                            FMenuType = MenuType.Function
                        };

                        adminDbContext.Permission.Add(homeIndex);

                        var systemCenter = new Permission
                        {
                            FName = "系统中心",
                            FRemark = "系统中心",
                            FSort = 1,
                            FMenuType = MenuType.TopMenu,
                            FTopMenuKey = "systemCenter"
                        };
                        adminDbContext.Permission.Add(systemCenter);

                        var freightCenter = new Permission
                        {
                            FName = "报价中心",
                            FRemark = "报价中心",
                            FSort = 2,
                            FMenuType = MenuType.TopMenu,
                            FTopMenuKey = "freightCenter"
                        };
                        adminDbContext.Permission.Add(freightCenter);

                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("初始化我的首页+初始化顶级菜单 完成");

                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "初始化我的首页+初始化顶级菜单 完成");

                        #region 初始化菜单组

                        Permission backendSystem = new Permission
                        {
                            FParentId = systemCenter.FId,
                            FSort = 0,
                            FRemark = "后台系统",
                            FName = "后台系统",
                            FIcon = "&#xe630;",
                            FMenuType = MenuType.MenuGroup
                        };
                        adminDbContext.Permission.Add(backendSystem);

                        // 添加业务系统
                        Permission businessSystem = new Permission
                        {
                            FParentId = systemCenter.FId,
                            FSort = 1,
                            FRemark = "业务系统",
                            FName = "业务系统",
                            FIcon = "&#xe630;",
                            FMenuType = MenuType.MenuGroup
                        };
                        adminDbContext.Permission.Add(businessSystem);

                        Permission freightSystem = new Permission
                        {
                            FParentId = freightCenter.FId,
                            FSort = 0,
                            FRemark = "报价系统",
                            FName = "报价系统",
                            FMenuType = MenuType.MenuGroup
                        };
                        adminDbContext.Permission.Add(freightSystem);

                        List<Permission> homeIndexFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "查看个人资料",
                                FControllerName = "Account",
                                FActionName = "Index",
                                FFullName = "Account_Index",
                                FUrl = "/Account/Index",
                                FRemark = "查看个人资料",
                                FParentId = homeIndex.FId,
                                FSort = 0,
                                FIcon = "icon-chakan",
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "修改自我密码",
                                FControllerName = "Account",
                                FActionName = "ChangePassword",
                                FFullName = "Account_ChangePassword",
                                FUrl = "/Account/ChangePassword",
                                FRemark = "修改自我密码",
                                FParentId = homeIndex.FId,
                                FSort = 1,
                                FIcon = "icon-chakan",
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(homeIndexFunctions);

                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("初始化菜单组 完成");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "初始化菜单组 完成");

                        #region 初始化功能

                        var adminManagerIndex = new Permission
                        {
                            FName = "管理员管理",
                            FAreaName = "Admin",
                            FControllerName = "Manager",
                            FActionName = "Index",
                            FFullName = "Admin_Manager_Index",
                            FUrl = "/Admin/Manager/Index",
                            FRemark = "管理员管理",
                            FParentId = backendSystem.FId,
                            FSort = 1,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(adminManagerIndex);

                        var adminPermissionIndex = new Permission
                        {
                            FName = "权限管理",
                            FAreaName = "Admin",
                            FControllerName = "Permission",
                            FActionName = "Index",
                            FFullName = "Admin_Permission_Index",
                            FUrl = "/Admin/Permission/Index",
                            FRemark = "权限管理",
                            FParentId = backendSystem.FId,
                            FSort = 3,
                            FIcon = "icon-vip",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(adminPermissionIndex);

                        var adminRoleIndex = new Permission
                        {
                            FName = "角色管理",
                            FAreaName = "Admin",
                            FControllerName = "Role",
                            FActionName = "Index",
                            FFullName = "Admin_Role_Index",
                            FUrl = "/Admin/Role/Index",
                            FRemark = "角色管理",
                            FParentId = backendSystem.FId,
                            FSort = 2,
                            FIcon = "&#xe612;",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(adminRoleIndex);

                        var freightHomeInquiry = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Home",
                            FActionName = "Inquiry",
                            FFullName = "Freight_Home_Inquiry",
                            FUrl = "/Freight/Home/Inquiry",
                            FRemark = "询价单管理",
                            FName = "询价单管理",
                            FParentId = freightSystem.FId,
                            FSort = 2,
                            FIcon = "&#xe612;",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightHomeInquiry);

                        var freightHomeIndex = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Home",
                            FActionName = "Index",
                            FFullName = "Freight_Home_Index",
                            FUrl = "/Freight/Home/Index",
                            FRemark = "报价总览",
                            FName = "报价总览",
                            FParentId = freightSystem.FId,
                            FSort = 0,
                            FIcon = "&#xe630;",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightHomeIndex);

                        var loginLogPermission = new Permission
                        {
                            FAreaName = "Admin",
                            FControllerName = "Home",
                            FActionName = "LoginLog",
                            FFullName = "Admin_Home_LoginLog",
                            FUrl = "/Admin/Home/LoginLog",
                            FRemark = "查看登陆日志",
                            FName = "查看登陆日志",
                            FParentId = backendSystem.FId,
                            FSort = 4,
                            FIcon = "icon-look",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(loginLogPermission);

                        var operationLogPermission = new Permission
                        {
                            FAreaName = "Admin",
                            FControllerName = "Home",
                            FActionName = "OperationLog",
                            FFullName = "Admin_Home_OperationLog",
                            FUrl = "/Admin/Home/OperationLog",
                            FRemark = "查看操作日志",
                            FName = "查看操作日志",
                            FParentId = backendSystem.FId,
                            FSort = 5,
                            FIcon = "icon-look",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(operationLogPermission);

                        var adminHomeIndex = new Permission
                        {
                            FAreaName = "Admin",
                            FControllerName = "Home",
                            FActionName = "Index",
                            FFullName = "Admin_Home_Index",
                            FUrl = "/Admin/Home/Index",
                            FRemark = "系统总览",
                            FName = "系统总览",
                            FParentId = backendSystem.FId,
                            FSort = 0,
                            FIcon = "&#xe630;",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(adminHomeIndex);

                        var freightHomeChannel = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Home",
                            FActionName = "Channel",
                            FFullName = "Freight_Home_Channel",
                            FUrl = "/Freight/Home/Channel",
                            FRemark = "渠道管理",
                            FName = "渠道管理",
                            FParentId = freightSystem.FId,
                            FSort = 1,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightHomeChannel);

                        var freightHomeQuote = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Home",
                            FActionName = "Quote",
                            FFullName = "Freight_Home_Quote",
                            FUrl = "/Freight/Home/Quote",
                            FRemark = "竞价单管理",
                            FName = "竞价单管理",
                            FParentId = freightSystem.FId,
                            FSort = 3,
                            FIcon = "icon-vip",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightHomeQuote);

                        var freightInquiryManageInquiryOrderStatusLog = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "InquiryManage",
                            FActionName = "InquiryOrderStatusLog",
                            FFullName = "Freight_InquiryManage_InquiryOrderStatusLog",
                            FUrl = "/Freight/InquiryManage/InquiryOrderStatusLog",
                            FRemark = "询价单状态日志",
                            FName = "询价单状态日志",
                            FParentId = freightSystem.FId,
                            FSort = 4,
                            FIcon = "icon-look",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightInquiryManageInquiryOrderStatusLog);

                        var freightCompanyIndex = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Company",
                            FActionName = "Index",
                            FFullName = "Freight_Company_Index",
                            FUrl = "/Freight/Company/Index",
                            FRemark = "运输商公司管理",
                            FName = "运输商公司管理",
                            FParentId = freightSystem.FId,
                            FSort = 5,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightCompanyIndex);

                        // 添加处理渠道举报功能
                        var freightReportIndex = new Permission
                        {
                            FAreaName = "Freight",
                            FControllerName = "Report",
                            FActionName = "Index",
                            FFullName = "Freight_Report_Index",
                            FUrl = "/Freight/Report/Index",
                            FRemark = "处理渠道举报",
                            FName = "处理渠道举报",
                            FParentId = freightSystem.FId,
                            FSort = 6,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightReportIndex);

                        // 添加17用户管理
                        var businessUserIndex = new Permission
                        {
                            FAreaName = "Business",
                            FControllerName = "User",
                            FActionName = "Index",
                            FFullName = "Business_User_Index",
                            FUrl = "/Business/User/Index",
                            FRemark = "用户管理",
                            FName = "用户管理",
                            FParentId = businessSystem.FId,
                            FSort = 0,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(businessUserIndex);

                        // 添加用户反馈管理
                        var businessUserFeedback = new Permission
                        {
                            FAreaName = "Business",
                            FControllerName = "User",
                            FActionName = "Feedback",
                            FFullName = "Business_User_Feedback",
                            FUrl = "/Business/User/Feedback",
                            FRemark = "反馈管理",
                            FName = "反馈管理",
                            FParentId = businessSystem.FId,
                            FSort = 1,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(businessUserFeedback);

                        // 添加查看日订单量折线图
                        var businessStatisticOrderByDay = new Permission
                        {
                            FAreaName = "Business",
                            FControllerName = "Statistic",
                            FActionName = "OrderByDay",
                            FFullName = "Business_Statistic_OrderByDay",
                            FUrl = "/Business/Statistic/OrderByDay",
                            FRemark = "日订单量",
                            FName = "日订单量",
                            FParentId = businessSystem.FId,
                            FSort = 2,
                            FIcon = "icon-chakan",
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(businessStatisticOrderByDay);

                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("功能权限初始化完成");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "功能权限初始化完成");

                        #region 具体页面功能权限初始化

                        var adminManagerFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "注册管理员",
                                FAreaName = "Admin",
                                FControllerName = "Manager",
                                FActionName = "Add",
                                FFullName = "Admin_Manager_Add",
                                FUrl = "/Admin/Manager/Add",
                                FRemark = "注册管理员",
                                FParentId = adminManagerIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "编辑管理员",
                                FAreaName = "Admin",
                                FControllerName = "Manager",
                                FActionName = "Edit",
                                FFullName = "Admin_Manager_Edit",
                                FUrl = "/Admin/Manager/Edit",
                                FRemark = "编辑管理员",
                                FParentId = adminManagerIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "获取管理员角色信息",
                                FAreaName = "Admin",
                                FControllerName = "Manager",
                                FActionName = "GetRoleList",
                                FFullName = "Admin_Manager_GetRoleList",
                                FUrl = "/Admin/Manager/GetRoleList",
                                FRemark = "获取管理员角色信息",
                                FParentId = adminManagerIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "设置管理员角色信息",
                                FAreaName = "Admin",
                                FControllerName = "Manager",
                                FActionName = "SetRoleList",
                                FFullName = "Admin_Manager_SetRoleList",
                                FUrl = "/Admin/Manager/SetRoleList",
                                FRemark = "设置管理员角色信息",
                                FParentId = adminManagerIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(adminManagerFunctions);

                        var adminPermissionFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "添加权限",
                                FAreaName = "Admin",
                                FControllerName = "Permission",
                                FActionName = "Add",
                                FFullName = "Admin_Permission_Add",
                                FUrl = "/Admin/Permission/Add",
                                FRemark = "添加权限",
                                FParentId = adminPermissionIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "编辑权限",
                                FAreaName = "Admin",
                                FControllerName = "Permission",
                                FActionName = "Edit",
                                FFullName = "Admin_Permission_Edit",
                                FUrl = "/Admin/Permission/Edit",
                                FRemark = "编辑权限",
                                FParentId = adminPermissionIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(adminPermissionFunctions);

                        var adminRoleFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "添加角色",
                                FAreaName = "Admin",
                                FControllerName = "Role",
                                FActionName = "Add",
                                FFullName = "Admin_Role_Add",
                                FUrl = "/Admin/Role/Add",
                                FRemark = "添加角色",
                                FParentId = adminRoleIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "编辑角色",
                                FAreaName = "Admin",
                                FControllerName = "Role",
                                FActionName = "Edit",
                                FFullName = "Admin_Role_Edit",
                                FUrl = "/Admin/Role/Edit",
                                FRemark = "编辑角色",
                                FParentId = adminRoleIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "查看角色权限",
                                FAreaName = "Admin",
                                FControllerName = "Role",
                                FActionName = "QueryPermissionList",
                                FFullName = "Admin_Role_QueryPermissionList",
                                FUrl = "/Admin/Role/QueryPermissionList",
                                FRemark = "查看角色权限",
                                FParentId = adminRoleIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "设置角色权限",
                                FAreaName = "Admin",
                                FControllerName = "Role",
                                FActionName = "SetPermissionList",
                                FFullName = "Admin_Role_SetPermissionList",
                                FUrl = "/Admin/Role/SetPermissionList",
                                FRemark = "设置角色权限",
                                FParentId = adminRoleIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(adminRoleFunctions);

                        Permission freightInquiryManageIndex = new Permission
                        {
                            FName = "下架询价单",
                            FAreaName = "Freight",
                            FControllerName = "InquiryManage",
                            FActionName = "Index",
                            FFullName = "Freight_InquiryManage_Index",
                            FUrl = "/Freight/InquiryManage/Index",
                            FRemark = "下架询价单",
                            FParentId = freightHomeInquiry.FId,
                            FSort = 0,
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightInquiryManageIndex);

                        Permission freightExportInquiryInfo = new Permission
                        {
                            FName = "导出询价单Excel",
                            FAreaName = "Freight",
                            FControllerName = "Export",
                            FActionName = "InquiryInfo",
                            FFullName = "Freight_Export_InquiryInfo",
                            FUrl = "/Freight/Export/InquiryInfo",
                            FRemark = "导出询价单Excel",
                            FParentId = freightHomeInquiry.FId,
                            FSort = 1,
                            FMenuType = MenuType.Function
                        };
                        adminDbContext.Permission.Add(freightExportInquiryInfo);

                        var freightHomeIndexFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "导出最近7天报价有更新的运输商",
                                FAreaName = "Freight",
                                FControllerName = "Export",
                                FActionName = "CarrierInfo",
                                FFullName = "Freight_Export_CarrierInfo",
                                FUrl = "/Freight/Export/CarrierInfo",
                                FRemark = "导出最近7天报价有更新的运输商",
                                FParentId = freightHomeIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "导出查询发布且未删除且没有过期的渠道信息",
                                FAreaName = "Freight",
                                FControllerName = "Export",
                                FActionName = "ValidChannelInfo",
                                FFullName = "Freight_Export_ValidChannelInfo",
                                FUrl = "/Freight/Export/ValidChannelInfo",
                                FRemark = "导出查询发布且未删除且没有过期的渠道信息",
                                FParentId = freightHomeIndex.FId,
                                FSort = 1,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "导出查询失效(自然过期或者停用)渠道信息,时间范围本周",
                                FAreaName = "Freight",
                                FControllerName = "Export",
                                FActionName = "InvalidChannelInfo",
                                FFullName = "Freight_Export_InvalidChannelInfo",
                                FUrl = "/Freight/Export/InvalidChannelInfo",
                                FRemark = "导出查询失效(自然过期或者停用)渠道信息,时间范围本周",
                                FParentId = freightHomeIndex.FId,
                                FSort = 2,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(freightHomeIndexFunctions);

                        var freightHomeChannelFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "查看渠道信息",
                                FAreaName = "Freight",
                                FControllerName = "Channel",
                                FActionName = "GetChannelPageData",
                                FFullName = "Freight_Channel_GetChannelPageData",
                                FUrl = "/Freight/Channel/GetChannelPageData",
                                FRemark = "查看渠道信息",
                                FParentId = freightHomeChannel.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "导出渠道Excel",
                                FAreaName = "Freight",
                                FControllerName = "Channel",
                                FActionName = "ExportChannelExcel",
                                FFullName = "Freight_Channel_ExportChannelExcel",
                                FUrl = "/Freight/Channel/ExportChannelExcel",
                                FRemark = "查看渠道信息",
                                FParentId = freightHomeChannel.FId,
                                FSort = 1,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(freightHomeChannelFunctions);

                        var freightHomeQuoteFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "导出竞价单Excel",
                                FAreaName = "Freight",
                                FControllerName = "Export",
                                FActionName = "QuoteInfo",
                                FFullName = "Freight_Export_QuoteInfo",
                                FUrl = "/Freight/Export/QuoteInfo",
                                FRemark = "导出竞价单Excel",
                                FParentId = freightHomeQuote.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(freightHomeQuoteFunctions);

                        // 运输商公司管理子权限
                        var freightCompanyIndexFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "查看审核",
                                FAreaName = "Freight",
                                FControllerName = "Company",
                                FActionName = "ViewCheck",
                                FFullName = "Freight_Company_ViewCheck",
                                FUrl = "/Freight/Company/ViewCheck",
                                FRemark = "查看审核",
                                FParentId = freightCompanyIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "修改资料",
                                FAreaName = "Freight",
                                FControllerName = "Company",
                                FActionName = "Edit",
                                FFullName = "Freight_Company_Edit",
                                FUrl = "/Freight/Company/Edit",
                                FRemark = "修改资料",
                                FParentId = freightCompanyIndex.FId,
                                FSort = 1,
                                FMenuType = MenuType.Function
                            },
                            new Permission
                            {
                                FName = "审核新注册运输商公司",
                                FAreaName = "Freight",
                                FControllerName = "Company",
                                FActionName = "Pass",
                                FFullName = "Freight_Company_Pass",
                                FUrl = "/Freight/Company/Pass",
                                FRemark = "审核新注册运输商公司",
                                FParentId = freightCompanyIndex.FId,
                                FSort = 2,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(freightCompanyIndexFunctions);

                        // 添加渠道举报处理子权限
                        var freightReportIndexFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "处理举报",
                                FAreaName = "Freight",
                                FControllerName = "Report",
                                FActionName = "Process",
                                FFullName = "Freight_Report_Process",
                                FUrl = "/Freight/Report/Process",
                                FRemark = "处理举报",
                                FParentId = freightReportIndex.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(freightReportIndexFunctions);

                        // 添加用户反馈子权限
                        var businessUserFeedbackFunctions = new List<Permission>
                        {
                            new Permission
                            {
                                FName = "回复用户反馈",
                                FAreaName = "Business",
                                FControllerName = "User",
                                FActionName = "Reply",
                                FFullName = "Business_User_Reply",
                                FUrl = "/Business/User/Reply",
                                FRemark = "回复用户反馈",
                                FParentId = businessUserFeedback.FId,
                                FSort = 0,
                                FMenuType = MenuType.Function
                            }
                        };
                        adminDbContext.Permission.AddRange(businessUserFeedbackFunctions);

                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("具体页面功能权限初始化完成");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "具体页面功能权限初始化完成");
                    }
                    else
                    {
                        logger.LogInformation("检测到当前数据库已经包含初始化权限");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "检测到当前数据库已经包含初始化权限");
                    }

                    int roleId = 0;

                    if (!adminDbContext.Role.Any())
                    {
                        #region 初始化默认超级管理角色赋予所有权限

                        var superRole = new Role
                        {
                            FName = "系统超级角色",
                            FRemark = "该角色拥有系统所有权限",
                            FIsActive = true,
                            FIsDeleted = false
                        };

                        adminDbContext.Role.Add(superRole);

                        adminDbContext.SaveChanges();

                        roleId = superRole.FId;

                        var rolePermissions = adminDbContext.Permission
                            .Where(x => x.FIsDeleted == false)
                            .Select(x => x.FId)
                            .ToList()
                            .Select(x => new RolePermission
                            {
                                FRoleId = superRole.FId,
                                FPermissionId = x
                            }).ToArray();

                        adminDbContext.RolePermission.AddRange(rolePermissions);

                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("初始化默认超级管理角色赋予所有权限完成");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "初始化默认超级管理角色赋予所有权限完成");
                    }
                    else
                    {
                        logger.LogInformation("检测到当前数据库已经包含初始化角色");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "检测到当前数据库已经包含初始化角色");
                    }

                    if (!adminDbContext.Manager.Any())
                    {
                        #region 初始化默认超级管理员赋予超级角色

                        var manager = new Manager
                        {
                            FAccount = "17track",
                            FNickName = "17track",
                            FPassword = "3D65F99E7238BC456CE9B17AA570DBAC", // 默认密码:17Track@
                            FRemark = "系统默认超级管理员",
                            FIsDeleted = false,
                            FIsLock = false
                        };

                        adminDbContext.Manager.Add(manager);
                        adminDbContext.SaveChanges();
                        adminDbContext.ManagerRole.Add(new ManagerRole
                        {
                            FManagerId = manager.FId,
                            FRoleId = roleId
                        });
                        adminDbContext.SaveChanges();

                        #endregion

                        logger.LogInformation("初始化默认超级管理员赋予超级角色完成");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "初始化默认超级管理员赋予超级角色完成");
                    }
                    else
                    {
                        logger.LogInformation("检测到当前数据库已经包含初始化管理员");
                        LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitDbDefaultData"), json: "检测到当前数据库已经包含初始化管理员");
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                    LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Error, "InitDbDefaultData"), json: "An error occurred seeding the DB.");
                }
            }
        }
    }
}