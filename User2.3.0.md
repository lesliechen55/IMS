## User2.3.0 支付发票申请 + IMS发票申请审核
> 此文档是针对这次迭代进行的补充发布文档，主要是IMS这边需要注意的地方。

### 需要修改的appsettings.json配置项：
* 数据库连接字符串：PayDbContext、ApiUserDbContext配置项后面添加：persist security info=true; 允许读取数据库连接字符串
* TSetting表新加UserQiniuConfig 七牛云图片配置
* 新加发票申请审核通过、发票申请驳回、线下交易审核通过、线下交易驳回 四个邮件模板：
```sql
--TTemplateType
INSERT [dbo].[TTemplateType] ([FTemplateTypeId], [FProjectId], [FChannel], [FTemplateName], [FTemplateDescribe], [FEnable], [FIsRendering], [FCreateTime], [FUpdateTime], [FDataJson], [FTemplateCode], [FTemplateTitle], [FTemplateBody]) VALUES (6599827160444436481, 4, 2, N'发票申请审核通过', N'发票申请审核通过', 1, NULL, CAST(N'2019-11-12T02:29:20.743' AS DateTime), CAST(N'2019-11-14T03:16:00.790' AS DateTime), N'{
	"MessageType": 100058,
	"TemplateData": "{
		\"InvoiceApplyId\":6598022967920427009
	}",
	"UserId": {
		"Token": null,
		"PushProvider": null,
		"uid": 6000000000000007810,
		"nid": 0,
		"dno": 0,
		"tno": 0,
		"ur": 2,
		"nn": "17TRACK",
		"e": "123456789@123.com",
		"s": 0,
		"ispay": 0,
		"lang": "zh-cn",
		"c": 0,
		"pho": 0,
		"level": 0
	},
	"SendTaskId": 0,
	"TransmitDataJson": null
}', 100058, N'<dict id="__0">Your invoice application has been approved</dict>', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        <dict id="__1">Your invoice application has been approved</dict>
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            <dict id="__2">Dear <tag id="_0_"><span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span></tag>,</dict>
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            <dict id="__3">I''m glad to inform you that your invoice application has been approved. Click the link below to view the delivery information:</dict>
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/invoice#od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank"><dict id="__4">Click to view details</dict></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__5">If you encounter problems, you can contact our customer service staff through
            QQ: <tag id="_1_"><a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a></tag> or email <tag id="_2_"><a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a></tag>, we will be happy to help you!</dict></p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__6">Best regards.</dict></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__7">17TRACK Team</dict></p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__8">This is an automatically generated email and sent from a notification-only address.</dict>
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__9">Please do not reply to this message!</dict>
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>')
GO
INSERT [dbo].[TTemplateType] ([FTemplateTypeId], [FProjectId], [FChannel], [FTemplateName], [FTemplateDescribe], [FEnable], [FIsRendering], [FCreateTime], [FUpdateTime], [FDataJson], [FTemplateCode], [FTemplateTitle], [FTemplateBody]) VALUES (6599830347788320769, 4, 2, N'发票申请驳回', N'发票申请驳回', 1, NULL, CAST(N'2019-11-12T02:42:00.657' AS DateTime), CAST(N'2019-11-14T03:16:30.737' AS DateTime), N'{
	"MessageType": 100059,
	"TemplateData": "{
		\"InvoiceApplyId\":6598027956411695105,
		\"RejectReason\":\"有点问题，你的发票信息裂开了\"
	}",
	"UserId": {
		"Token": null,
		"PushProvider": null,
		"uid": 6000000000000007810,
		"nid": 0,
		"dno": 0,
		"tno": 0,
		"ur": 2,
		"nn": "17TRACK",
		"e": "123456789@123.com",
		"s": 0,
		"ispay": 0,
		"lang": "zh-cn",
		"c": 0,
		"pho": 0,
		"level": 0
	},
	"SendTaskId": 0,
	"TransmitDataJson": null
}', 100059, N'<dict id="__0">Your invoice request has been rejected</dict>', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        <dict id="__1">Your invoice request has been rejected</dict>
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            <dict id="__2">Dear <tag id="_0_"><span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span></tag>,</dict>
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            <dict id="__3">We regret to inform you that your invoice application has not been approved for the following reasons:</dict><br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/invoice#od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank"><dict id="__4">Click to view details</dict></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__5">We dismissed it, please refer to the reason for the rejection and reapply.</dict><dict id="__6">If you encounter problems, you can contact our customer service staff through
            QQ: <tag id="_1_"><a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a></tag> or email <tag id="_2_"><a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a></tag>, we will be happy to help you!</dict></p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__7">Best regards.</dict></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__8">17TRACK Team</dict></p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__9">This is an automatically generated email and sent from a notification-only address.</dict>
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__10">Please do not reply to this message!</dict>
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>')
GO
INSERT [dbo].[TTemplateType] ([FTemplateTypeId], [FProjectId], [FChannel], [FTemplateName], [FTemplateDescribe], [FEnable], [FIsRendering], [FCreateTime], [FUpdateTime], [FDataJson], [FTemplateCode], [FTemplateTitle], [FTemplateBody]) VALUES (6599831388781019137, 4, 2, N'线下交易审核通过', N'线下交易审核通过', 1, NULL, CAST(N'2019-11-12T02:46:08.847' AS DateTime), CAST(N'2019-11-14T03:16:44.560' AS DateTime), N'{
	"MessageType": 100060,
	"TemplateData": "{
		\"OfflinePaymentId\":6598079454097178625
	}",
	"UserId": {
		"Token": null,
		"PushProvider": null,
		"uid": 6000000000000007810,
		"nid": 0,
		"dno": 0,
		"tno": 0,
		"ur": 2,
		"nn": "17TRACK",
		"e": "123456789@123.com",
		"s": 0,
		"ispay": 0,
		"lang": "zh-cn",
		"c": 0,
		"pho": 0,
		"level": 0
	},
	"SendTaskId": 0,
	"TransmitDataJson": null
}', 100060, N'<dict id="__0">Your offline transaction has been approved</dict>', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        <dict id="__1">Your offline transaction has been approved</dict>
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            <dict id="__2">Dear <tag id="_0_"><span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span></tag>,</dict>
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            <dict id="__3">I''m glad to inform you that your offline transaction has been approved. Click the link below to view the effective order information:</dict>
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank"><dict id="__4">Click to view details</dict></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__5">If you encounter problems, you can contact our customer service staff through
            QQ: <tag id="_1_"><a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a></tag> or email <tag id="_2_"><a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a></tag>, we will be happy to help you!</dict></p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__6">Best regards.</dict></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__7">17TRACK Team</dict></p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__8">This is an automatically generated email and sent from a notification-only address.</dict>
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__9">Please do not reply to this message!</dict>
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>')
GO
INSERT [dbo].[TTemplateType] ([FTemplateTypeId], [FProjectId], [FChannel], [FTemplateName], [FTemplateDescribe], [FEnable], [FIsRendering], [FCreateTime], [FUpdateTime], [FDataJson], [FTemplateCode], [FTemplateTitle], [FTemplateBody]) VALUES (6599832728403968001, 4, 2, N'线下交易驳回', N'线下交易驳回', 1, NULL, CAST(N'2019-11-12T02:51:28.240' AS DateTime), CAST(N'2019-11-14T03:17:02.160' AS DateTime), N'{
	"MessageType": 100061,
	"TemplateData": "{
		\"OfflinePaymentId\":6598103018129063937,
		\"RejectReason\":\"有点问题，你的交易凭证裂开了\"
	}",
	"UserId": {
		"Token": null,
		"PushProvider": null,
		"uid": 6000000000000007810,
		"nid": 0,
		"dno": 0,
		"tno": 0,
		"ur": 2,
		"nn": "17TRACK",
		"e": "123456789@123.com",
		"s": 0,
		"ispay": 0,
		"lang": "zh-cn",
		"c": 0,
		"pho": 0,
		"level": 0
	},
	"SendTaskId": 0,
	"TransmitDataJson": null
}', 100061, N'<dict id="__0">Your offline transaction has been rejected</dict>', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        <dict id="__1">Your offline transaction has been rejected</dict>
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            <dict id="__2">Dear <tag id="_0_"><span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span></tag>,</dict>
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            <dict id="__3">We are sorry to inform you that your offline transaction has not been approved for the following reasons:</dict><br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank"><dict id="__4">Click to view details</dict></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__5">We dismissed it, please refer to the reason for the rejection and reapply.</dict><dict id="__6">If you encounter problems, you can contact our customer service staff through
            QQ: <tag id="_1_"><a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a></tag> or email <tag id="_2_"><a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a></tag>, we will be happy to help you!</dict></p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__7">Best regards.</dict></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"><dict id="__8">17TRACK Team</dict></p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__9">This is an automatically generated email and sent from a notification-only address.</dict>
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                <dict id="__10">Please do not reply to this message!</dict>
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>')

--TTemplate
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599848511171461121, 6599830347788320769, N'en', N'Your invoice request has been rejected', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        Your invoice request has been rejected
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            Dear <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            We regret to inform you that your invoice application has not been approved for the following reasons:<br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/invoice#p=1&od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">Click to view details</a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">We dismissed it, please refer to the reason for the rejection and reapply.If you encounter problems, you can contact our customer service staff through QQ: <a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> or email <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a>, we will be happy to help you!</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">Best regards.</p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK Team</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                This is an automatically generated email and sent from a notification-only address.
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                Please do not reply to this message!
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100059,
  "templateTypeId": "6599830347788320769",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your invoice request has been rejected",
      "target": "Your invoice request has been rejected"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your invoice request has been rejected",
      "target": "Your invoice request has been rejected"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "Dear {0},"
    },
    {
      "id": "__3",
      "source": "We regret to inform you that your invoice application has not been approved for the following reasons:",
      "target": "We regret to inform you that your invoice application has not been approved for the following reasons:"
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "Click to view details"
    },
    {
      "id": "__5",
      "source": "We dismissed it, please refer to the reason for the rejection and reapply.",
      "target": "We dismissed it, please refer to the reason for the rejection and reapply."
    },
    {
      "id": "__6",
      "source": "If you encounter problems, you can contact our customer service staff through\n            QQ: {1} or email {2}, we will be happy to help you!",
      "target": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!"
    },
    {
      "id": "__7",
      "source": "Best regards.",
      "target": "Best regards."
    },
    {
      "id": "__8",
      "source": "17TRACK Team",
      "target": "17TRACK Team"
    },
    {
      "id": "__9",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "This is an automatically generated email and sent from a notification-only address."
    },
    {
      "id": "__10",
      "source": "Please do not reply to this message!",
      "target": "Please do not reply to this message!"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599849587622805505, 6599830347788320769, N'zh-cn', N'你的发票申请已经被驳回', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        你的发票申请已经被驳回
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            你好 <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            很遗憾地通知你，你的发票申请因为以下原因没有通过审核：<br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/invoice#p=1&od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">查看详情>></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">我们将它驳回了，请参考驳回原因修改后重新上传。如果遇到问题可以通过QQ：<a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> 或者邮箱 <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a> 联系我们的客服人员，我们将竭诚为你服务！</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK 团队</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                邮件为系统自动发送
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                无需回复！
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100059,
  "templateTypeId": "6599830347788320769",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your invoice request has been rejected",
      "target": "你的发票申请已经被驳回"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your invoice request has been rejected",
      "target": "你的发票申请已经被驳回"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "你好 {0},"
    },
    {
      "id": "__3",
      "source": "We regret to inform you that your invoice application has not been approved for the following reasons:",
      "target": "很遗憾地通知你，你的发票申请因为以下原因没有通过审核："
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "查看详情>>"
    },
    {
      "id": "__5",
      "source": "We dismissed it, please refer to the reason for the rejection and reapply.",
      "target": "我们将它驳回了，请参考驳回原因修改后重新上传。"
    },
    {
      "id": "__6",
      "source": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!",
      "target": "如果遇到问题可以通过QQ：{1} 或者邮箱 {2} 联系我们的客服人员，我们将竭诚为你服务！"
    },
    {
      "id": "__7",
      "source": "Best regards.",
      "target": ""
    },
    {
      "id": "__8",
      "source": "17TRACK Team",
      "target": "17TRACK 团队"
    },
    {
      "id": "__9",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "邮件为系统自动发送"
    },
    {
      "id": "__10",
      "source": "Please do not reply to this message!",
      "target": "无需回复！"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599850051839983617, 6599827160444436481, N'en', N'Your invoice application has been approved', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        Your invoice application has been approved
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            Dear <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            I''m glad to inform you that your invoice application has been approved. Click the link below to view the delivery information:
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/invoice#od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">Click to view details</a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">If you encounter problems, you can contact our customer service staff through QQ: <a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> or email <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a>, we will be happy to help you!</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">Best regards.</p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK Team</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                This is an automatically generated email and sent from a notification-only address.
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                Please do not reply to this message!
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100058,
  "templateTypeId": "6599827160444436481",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your invoice application has been approved",
      "target": "Your invoice application has been approved"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your invoice application has been approved",
      "target": "Your invoice application has been approved"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "Dear {0},"
    },
    {
      "id": "__3",
      "source": "I''m glad to inform you that your invoice application has been approved. Click the link below to view the delivery information:",
      "target": "I''m glad to inform you that your invoice application has been approved. Click the link below to view the delivery information:"
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "Click to view details"
    },
    {
      "id": "__5",
      "source": "If you encounter problems, you can contact our customer service staff through\n            QQ: {1} or email {2}, we will be happy to help you!",
      "target": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!"
    },
    {
      "id": "__6",
      "source": "Best regards.",
      "target": "Best regards."
    },
    {
      "id": "__7",
      "source": "17TRACK Team",
      "target": "17TRACK Team"
    },
    {
      "id": "__8",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "This is an automatically generated email and sent from a notification-only address."
    },
    {
      "id": "__9",
      "source": "Please do not reply to this message!",
      "target": "Please do not reply to this message!"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599850908727902209, 6599827160444436481, N'zh-cn', N'你的发票申请已经审核通过', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        你的发票申请已经审核通过
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            你好 <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            很高兴地通知你，你的发票申请审核通过了, 点击下方链接查看派送信息：
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/invoice#od=@string.Format("{0}",  data["InvoiceApplyId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">查看详情>></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">如果遇到问题可以通过QQ：<a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> 或者邮箱 <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a> 联系我们的客服人员，我们将竭诚为你服务！</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK 团队</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                邮件为系统自动发送
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                无需回复！
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100058,
  "templateTypeId": "6599827160444436481",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your invoice application has been approved",
      "target": "你的发票申请已经审核通过"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your invoice application has been approved",
      "target": "你的发票申请已经审核通过"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "你好 {0},"
    },
    {
      "id": "__3",
      "source": "I''m glad to inform you that your invoice application has been approved. Click the link below to view the delivery information:",
      "target": "很高兴地通知你，你的发票申请审核通过了, 点击下方链接查看派送信息："
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "查看详情>>"
    },
    {
      "id": "__5",
      "source": "If you encounter problems, you can contact our customer service staff through\n            QQ: {1} or email {2}, we will be happy to help you!",
      "target": "如果遇到问题可以通过QQ：{1} 或者邮箱 {2} 联系我们的客服人员，我们将竭诚为你服务！"
    },
    {
      "id": "__6",
      "source": "Best regards.",
      "target": ""
    },
    {
      "id": "__7",
      "source": "17TRACK Team",
      "target": "17TRACK 团队"
    },
    {
      "id": "__8",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "邮件为系统自动发送"
    },
    {
      "id": "__9",
      "source": "Please do not reply to this message!",
      "target": "无需回复！"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599876915778289665, 6599831388781019137, N'en', N'Your offline transaction has been approved', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        Your offline transaction has been approved
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            Dear <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            I''m glad to inform you that your offline transaction has been approved. Click the link below to view the effective order information:
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",  data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">Click to view details</a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">If you encounter problems, you can contact our customer service staff through QQ: <a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> or email <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a>, we will be happy to help you!</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">Best regards.</p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK Team</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                This is an automatically generated email and sent from a notification-only address.
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                Please do not reply to this message!
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100060,
  "templateTypeId": "6599831388781019137",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your offline transaction has been approved",
      "target": "Your offline transaction has been approved"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your offline transaction has been approved",
      "target": "Your offline transaction has been approved"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "Dear {0},"
    },
    {
      "id": "__3",
      "source": "I''m glad to inform you that your offline transaction has been approved. Click the link below to view the effective order information:",
      "target": "I''m glad to inform you that your offline transaction has been approved. Click the link below to view the effective order information:"
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "Click to view details"
    },
    {
      "id": "__5",
      "source": "If you encounter problems, you can contact our customer service staff through\n            QQ: {1} or email {2}, we will be happy to help you!",
      "target": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!"
    },
    {
      "id": "__6",
      "source": "Best regards.",
      "target": "Best regards."
    },
    {
      "id": "__7",
      "source": "17TRACK Team",
      "target": "17TRACK Team"
    },
    {
      "id": "__8",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "This is an automatically generated email and sent from a notification-only address."
    },
    {
      "id": "__9",
      "source": "Please do not reply to this message!",
      "target": "Please do not reply to this message!"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599878577251155969, 6599831388781019137, N'zh-cn', N'你的线下交易已经审核通过', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAAAgCAYAAABXTzdxAAAIPUlEQVR42uWcCYwURRSGH4ccKhovjiiKaFQUr6hgpmaGFSIIghojxAujKHiERIlCSMQLMSGyLEdAxH4zA7scZiRqXECUCAaVeKwEReJJQAIsp8u9Asu2/QeTHXZm+lX3TDe9zUtewlH1pqr6fXW+KsoUs4zGmaVkOtIp1ImQdyr1xN990ck0jBqL4q8pYpge6l6Kzj+PILGKTqSMAy5sHLJ0i6U/k+LPSSXGW3bupL7lZ1GxJMKLHZbpMMWS/ciNlPCVpHiSpUtIGb9ThI84+22uJ8WbLV1BUX6PVOIOspO40Q3lzWtPcZp0RCWeQVlFjfKETDjaWo63M+CAAI4tZppaUaZEE1E0kPc65wqCxFPXF9Wu4v0ER+s992IqRErmXeKuDLyQnEg82ZkUV1haV/Q2RkeH75lDAJCQ9wcNqLuT4lqNNvmORlSdkQnISDhg4AEpo1FCrxlUQGRVxr+k+ClyKcjrEpDd9LrZnHQkmuyKUdDjtj5KkeS9RQdkcLotKV6v0R77UM8GOFZSS8v5NgUekMm0x5xJZ1ODCM4aSEBkjfI7LgFZ5P43kz01f+MrX9pacQ31MToUExBM47R+O8YPUqZYjvcoHLAJjCBvZH8wY17oAIEqftXZ9GplSziV+99MvCaPHsYtSOuf8pgiAYK8QzRHcT4ZDpOaWY63LvCATKZD5gy64OTRo/xyzIPDBkjDAjZxKwlStHWY4tUkSSQx3FdAFL9fFEBKUl2wyaLxm7/SoMozG48eA+GAgQekjKbmWHvMRMVCCQhU8af6gPAEqWcUdoDqsEsn7P6MRlofdZkrQLJH1m812rqWYsYNBGkEyCrLAY/bqB4gZdQD6V3pZKoXRo+jlnamTIklLyLFB63KHZeV68UGkm3sgQP5CgjK3be8PemIMqrsbMG5AJyQZkhxAJG/iW+ARHii5rrvOXIhAOiACIhb0d9Bm0OFiErcIzTsSnIgMiC8Gz1Xlg5OtyJskcY5bqVJaJ0bRPlhEgSdhW0ngDMb/HaUnxfWIckCAdlBas51pCMAXxnLvQQEZ0x6nSN/RJAAAiLvoGF0mULdmhogmr3+ALE3xdRJrt8jQv0++T/d1UK6rQUBAugdCOD3DBAAGOHtGlOrzZgZBBeQKTRUmF6BbgolIJCI8YHwEWfL9eMK7elDhDfapUXdXAOijFnkRGLJBzwBxDSbIa8GHHUNh5IBBAQ7aJb9X+zsY/EfbkB4jPAhFwiNCGfYIZxxdM2A6V27tICgiQMit2lDeV8hSGABKaNBwtpDcNwQAKL4JcnpCjub4D9Odkq+T3CcL/wCRC4LL3YMSIx74CRehoNXInogyIDA9jfC1m7f0wCQtNCjjxemaC8LTju9UVu0s3cgPoLgSV8AQehHlIdadp/MqbHktY4AQcCk4g0aI8euhri3YAKCc5O4sPZYQ5AwA4IoWnmRPlhY6K8SwibuzgHVl8JvDvIWEH0RAHEbVjOQIIEGpJSWCqPHkDACgh0TnJAj3grBieJ5Qj8+n/JJ/3nnWDaO2QY/4mQ4e1o2Vij/zPACwlMJEmBAYPNGYfT40zSpeaP5qgoWIPLJbLZmAyE43IfC6HG/kH957vLzzUK+v0ILiGIj+ICU0kIBkOE5pgWVQQPEU1Vch7sLBUWnRo0X8+58Ka62y4vLUCEARIgYCCAgVvquls06Gzi2mdOpdVYgnuKa0wuQxAtyvYy/pQhdHMblUuz4CG0y0nNA+i9tja3efGUEpB4AAt2LQNdgAlJKs4S1x+hcF6LCB4i8BpCun3pchsVeA4JLUUI5Kj0CxEQAI8J/ggQIdq46WHlqbUaPGjNB7bKdkesLAQS7Ml4D4v/owaM8nuIdRPxWkA8KC1eeGCRAYGuisPaYkO9CVNgAER1DBuQzz6d5Me4TckDqEdAYCEDM2XSuZWufDRyHzVnUPt+FqCY2xTpOyvgnW3mrpestXY162e2A4RBNuFtd68NGwSQBECEaWA6y9BiQwxp1rEZg46kHpJTGCqPHDJsLUU0MEN5IgojPFEX5Lpv69PfpPso6hyPILpzxYGomarT8UhxYegcI1q2pmOaN02XY2TtlgJgpamPZ2W5j55hlp0vWfYGGHsDrKZb/gER5nNsDLfwf0vihCMkI4o1CMWy/ZMGFBEFAokYe1M07QGQbzwqjR0WOtcdbKHhoR5Beqduk+9E2o89vfjkpYqKaGCDHT3qADgGJ+L5yvqMIdPQdEDNNLSwbG2wvRE2j7tkhFFwTZkDw4TAlse+9+bJcDxCIe/yKKxzoCqFt0sK7Wx4qL3QOCL9JjQWjIMJ/5JFnA3zPX0BK6SFh9KgU7kiEExCI4vlCuZ7O9WymkGc+ORA4j/g+1eB0C4JEUjd5B4McCQBARAdHWXMIAhR1ofQTEORfK+RXWaeriqtPB0AQ6u34rrQyPrbNA5sOBW8F298yTEYyoF7h0+ixB+sINxemCl67RXmYH4Ag7wBh9FiVI9J0BAp5OgCClwPtH1vg/Znvw+LP+LeCX0HJhu5t3TspJ3afeJOXcCCoEzt1Hjw92ooivEbrkfGS1DV+ALJKAGRAowq0QCRpeACRRfxgcY5nHKz1Ej7sj+RG4snegt3vG62DOp4IlOQjHsCxHBsYnj1eHZ17ldbL/Ip/surZRgeQnQgPyaszqWOe67Q9bPJBq3KGbyuuzaPV5FIwdwZg+Ww7fgsXPb/inTZlXUKaghETHyyvrUji8Yy0T+RNBxt4+dCNoGdVRpWN7W15X3nHoljxIkvXYoMA6bUVPTVuAaK9IolppFK3S86NqVd+e1yu+QrlY3rlS5T+B+x3RySjOKIrAAAAAElFTkSuQmCC" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        你的线下交易已经审核通过
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            你好 <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>，
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            很高兴地通知你，你的线下交易审核通过了, 点击下方链接查看生效的订单信息：
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",  data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">查看详情>></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">如果遇到问题可以通过QQ：<a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> 或者邮箱 <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a> 联系我们的客服人员，我们将竭诚为你服务！</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK 团队</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                邮件为系统自动发送
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                无需回复！
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100060,
  "templateTypeId": "6599831388781019137",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your offline transaction has been approved",
      "target": "你的线下交易已经审核通过"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your offline transaction has been approved",
      "target": "你的线下交易已经审核通过"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "你好 {0}，"
    },
    {
      "id": "__3",
      "source": "I''m glad to inform you that your offline transaction has been approved. Click the link below to view the effective order information:",
      "target": "很高兴地通知你，你的线下交易审核通过了, 点击下方链接查看生效的订单信息："
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "查看详情>>"
    },
    {
      "id": "__5",
      "source": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!",
      "target": "如果遇到问题可以通过QQ：{1} 或者邮箱 {2} 联系我们的客服人员，我们将竭诚为你服务！"
    },
    {
      "id": "__6",
      "source": "Best regards.",
      "target": ""
    },
    {
      "id": "__7",
      "source": "17TRACK Team",
      "target": "17TRACK 团队"
    },
    {
      "id": "__8",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "邮件为系统自动发送"
    },
    {
      "id": "__9",
      "source": "Please do not reply to this message!",
      "target": "无需回复！"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599879148225953793, 6599832728403968001, N'en', N'Your offline transaction has been rejected', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAAAgCAYAAABXTzdxAAAIPUlEQVR42uWcCYwURRSGH4ccKhovjiiKaFQUr6hgpmaGFSIIghojxAujKHiERIlCSMQLMSGyLEdAxH4zA7scZiRqXECUCAaVeKwEReJJQAIsp8u9Asu2/QeTHXZm+lX3TDe9zUtewlH1pqr6fXW+KsoUs4zGmaVkOtIp1ImQdyr1xN990ck0jBqL4q8pYpge6l6Kzj+PILGKTqSMAy5sHLJ0i6U/k+LPSSXGW3bupL7lZ1GxJMKLHZbpMMWS/ciNlPCVpHiSpUtIGb9ThI84+22uJ8WbLV1BUX6PVOIOspO40Q3lzWtPcZp0RCWeQVlFjfKETDjaWo63M+CAAI4tZppaUaZEE1E0kPc65wqCxFPXF9Wu4v0ER+s992IqRErmXeKuDLyQnEg82ZkUV1haV/Q2RkeH75lDAJCQ9wcNqLuT4lqNNvmORlSdkQnISDhg4AEpo1FCrxlUQGRVxr+k+ClyKcjrEpDd9LrZnHQkmuyKUdDjtj5KkeS9RQdkcLotKV6v0R77UM8GOFZSS8v5NgUekMm0x5xJZ1ODCM4aSEBkjfI7LgFZ5P43kz01f+MrX9pacQ31MToUExBM47R+O8YPUqZYjvcoHLAJjCBvZH8wY17oAIEqftXZ9GplSziV+99MvCaPHsYtSOuf8pgiAYK8QzRHcT4ZDpOaWY63LvCATKZD5gy64OTRo/xyzIPDBkjDAjZxKwlStHWY4tUkSSQx3FdAFL9fFEBKUl2wyaLxm7/SoMozG48eA+GAgQekjKbmWHvMRMVCCQhU8af6gPAEqWcUdoDqsEsn7P6MRlofdZkrQLJH1m812rqWYsYNBGkEyCrLAY/bqB4gZdQD6V3pZKoXRo+jlnamTIklLyLFB63KHZeV68UGkm3sgQP5CgjK3be8PemIMqrsbMG5AJyQZkhxAJG/iW+ARHii5rrvOXIhAOiACIhb0d9Bm0OFiErcIzTsSnIgMiC8Gz1Xlg5OtyJskcY5bqVJaJ0bRPlhEgSdhW0ngDMb/HaUnxfWIckCAdlBas51pCMAXxnLvQQEZ0x6nSN/RJAAAiLvoGF0mULdmhogmr3+ALE3xdRJrt8jQv0++T/d1UK6rQUBAugdCOD3DBAAGOHtGlOrzZgZBBeQKTRUmF6BbgolIJCI8YHwEWfL9eMK7elDhDfapUXdXAOijFnkRGLJBzwBxDSbIa8GHHUNh5IBBAQ7aJb9X+zsY/EfbkB4jPAhFwiNCGfYIZxxdM2A6V27tICgiQMit2lDeV8hSGABKaNBwtpDcNwQAKL4JcnpCjub4D9Odkq+T3CcL/wCRC4LL3YMSIx74CRehoNXInogyIDA9jfC1m7f0wCQtNCjjxemaC8LTju9UVu0s3cgPoLgSV8AQehHlIdadp/MqbHktY4AQcCk4g0aI8euhri3YAKCc5O4sPZYQ5AwA4IoWnmRPlhY6K8SwibuzgHVl8JvDvIWEH0RAHEbVjOQIIEGpJSWCqPHkDACgh0TnJAj3grBieJ5Qj8+n/JJ/3nnWDaO2QY/4mQ4e1o2Vij/zPACwlMJEmBAYPNGYfT40zSpeaP5qgoWIPLJbLZmAyE43IfC6HG/kH957vLzzUK+v0ILiGIj+ICU0kIBkOE5pgWVQQPEU1Vch7sLBUWnRo0X8+58Ka62y4vLUCEARIgYCCAgVvquls06Gzi2mdOpdVYgnuKa0wuQxAtyvYy/pQhdHMblUuz4CG0y0nNA+i9tja3efGUEpB4AAt2LQNdgAlJKs4S1x+hcF6LCB4i8BpCun3pchsVeA4JLUUI5Kj0CxEQAI8J/ggQIdq46WHlqbUaPGjNB7bKdkesLAQS7Ml4D4v/owaM8nuIdRPxWkA8KC1eeGCRAYGuisPaYkO9CVNgAER1DBuQzz6d5Me4TckDqEdAYCEDM2XSuZWufDRyHzVnUPt+FqCY2xTpOyvgnW3mrpestXY162e2A4RBNuFtd68NGwSQBECEaWA6y9BiQwxp1rEZg46kHpJTGCqPHDJsLUU0MEN5IgojPFEX5Lpv69PfpPso6hyPILpzxYGomarT8UhxYegcI1q2pmOaN02XY2TtlgJgpamPZ2W5j55hlp0vWfYGGHsDrKZb/gER5nNsDLfwf0vihCMkI4o1CMWy/ZMGFBEFAokYe1M07QGQbzwqjR0WOtcdbKHhoR5Beqduk+9E2o89vfjkpYqKaGCDHT3qADgGJ+L5yvqMIdPQdEDNNLSwbG2wvRE2j7tkhFFwTZkDw4TAlse+9+bJcDxCIe/yKKxzoCqFt0sK7Wx4qL3QOCL9JjQWjIMJ/5JFnA3zPX0BK6SFh9KgU7kiEExCI4vlCuZ7O9WymkGc+ORA4j/g+1eB0C4JEUjd5B4McCQBARAdHWXMIAhR1ofQTEORfK+RXWaeriqtPB0AQ6u34rrQyPrbNA5sOBW8F298yTEYyoF7h0+ixB+sINxemCl67RXmYH4Ag7wBh9FiVI9J0BAp5OgCClwPtH1vg/Znvw+LP+LeCX0HJhu5t3TspJ3afeJOXcCCoEzt1Hjw92ooivEbrkfGS1DV+ALJKAGRAowq0QCRpeACRRfxgcY5nHKz1Ej7sj+RG4snegt3vG62DOp4IlOQjHsCxHBsYnj1eHZ17ldbL/Ip/surZRgeQnQgPyaszqWOe67Q9bPJBq3KGbyuuzaPV5FIwdwZg+Ww7fgsXPb/inTZlXUKaghETHyyvrUji8Yy0T+RNBxt4+dCNoGdVRpWN7W15X3nHoljxIkvXYoMA6bUVPTVuAaK9IolppFK3S86NqVd+e1yu+QrlY3rlS5T+B+x3RySjOKIrAAAAAElFTkSuQmCC" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        Your offline transaction has been rejected
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            Dear <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>,
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            We are sorry to inform you that your offline transaction has not been approved for the following reasons:<br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">Click to view details</a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">We dismissed it, please refer to the reason for the rejection and reapply.If you encounter problems, you can contact our customer service staff through QQ: <a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> or email <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a>, we will be happy to help you!</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">Best regards.</p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK Team</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                This is an automatically generated email and sent from a notification-only address.
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                Please do not reply to this message!
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100061,
  "templateTypeId": "6599832728403968001",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your offline transaction has been rejected",
      "target": "Your offline transaction has been rejected"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your offline transaction has been rejected",
      "target": "Your offline transaction has been rejected"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "Dear {0},"
    },
    {
      "id": "__3",
      "source": "We are sorry to inform you that your offline transaction has not been approved for the following reasons:",
      "target": "We are sorry to inform you that your offline transaction has not been approved for the following reasons:"
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "Click to view details"
    },
    {
      "id": "__5",
      "source": "We dismissed it, please refer to the reason for the rejection and reapply.",
      "target": "We dismissed it, please refer to the reason for the rejection and reapply."
    },
    {
      "id": "__6",
      "source": "If you encounter problems, you can contact our customer service staff through\n            QQ: {1} or email {2}, we will be happy to help you!",
      "target": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!"
    },
    {
      "id": "__7",
      "source": "Best regards.",
      "target": "Best regards."
    },
    {
      "id": "__8",
      "source": "17TRACK Team",
      "target": "17TRACK Team"
    },
    {
      "id": "__9",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "This is an automatically generated email and sent from a notification-only address."
    },
    {
      "id": "__10",
      "source": "Please do not reply to this message!",
      "target": "Please do not reply to this message!"
    }
  ]
}')
GO
INSERT [dbo].[TTemplate] ([FTemplateId], [FTemplateTypeId], [FLanguage], [FTemplateTitle], [FTemplateBody], [FIsDel], [FTemplateData]) VALUES (6599881641622241281, 6599832728403968001, N'zh-cn', N'你的线下交易已经被驳回', N'@{
    JObject data = YQTrackV6.Common.Utils.SerializeExtend.MyDeserializeFromJson<JObject>(Model.MessageData);
}
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<head style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta name="viewport" content="width=device-width" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
<title style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">email | 17TRACK</title>
</head>

<body itemscope itemtype="http://schema.org/EmailMessage" style="-webkit-font-smoothing: antialiased; -webkit-text-size-adjust: none; background-color: #fff; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; height: 100%; line-height: 1.5; margin: 0; padding: 10px; width: 100% !important;">

<table class="body-wrap" style="background-color: #f1f3fa; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; width: 100%;">
	<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
		<!-- width="480" -->
		<td class="container" style="box-sizing: border-box; clear: both !important; display: block !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto !important; max-width: 480px !important; vertical-align: top;">
			<div class="content" style="box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px;">
				
				<div class="header" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; margin-bottom: 20px; margin-top: 30px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-align: center; vertical-align: top;"><img alt="BG" src="data:image/png;base64,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" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></td>
                        </tr>
                    </table>
                </div>
				
				<table class="main" width="100%" cellpadding="0" cellspacing="0" style="background-color: #fff; border: 1px solid #e9e9e9; border-radius: 3px; box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
					<tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
						<td class="content-wrap" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 15px; vertical-align: top;">
                            
<div class="audit-info" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
    <div class="main-title bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; color: #212121; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 18px; margin: 0; padding: 5px 0 20px 0; text-align: center;">
        <!-- replace:start: rejectedWords -->
        你的线下交易已经被驳回
        <!-- replace:end: rejectedWords -->
    </div>
    <div class="content bottom-line" style="border-bottom: 1px solid rgba(0, 0, 0, 0.12); box-sizing: border-box; display: block; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0 auto; max-width: 480px; padding: 20px 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:nameWords -->
            你好 <span class="orange-500" style="box-sizing: border-box; color: #ff8c00 !important; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">@StringHelper.StrHtmlEncode(Model.UserInfo.FEmail)</span>，
            <!-- replace:end:nameWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <!-- replace:start:plsWords -->
            很遗憾地通知你，你的线下交易因为以下原因没有通过审核：<br>
            @string.Format("{0}",  data["RejectReason"])
            <!-- replace:end:plsWords -->
        </p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <!-- replace:start:detailLinkWords -->
        <a href="https://user.17track.net/@string.Format("{0}",  Model.UserInfo.FLanguage)/offline-trading#od=@string.Format("{0}",data["OfflinePaymentId"])" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;" target="_blank">查看详情>></a>
        <!-- replace:end:detailLinkWords -->
    </div>
    <div class="description text-left" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding-top: 20px; text-align: left !important;">
        <!-- replace:start:prompt -->
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">我们将它驳回了，请参考驳回原因修改后重新上传。如果遇到问题可以通过QQ：<a href="tencent://message/?uin=3369584109&Site=&Menu=yes" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">3369584109</a> 或者邮箱 <a href="mailto:serv@17track.net" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; text-decoration: underline;">serv@17track.net</a> 联系我们的客服人员，我们将竭诚为你服务！</p>
        <br style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;"></p>
        <p style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">17TRACK 团队</p>
        <!-- replace:end:prompt -->
    </div>
</div>

						</td>
					</tr>
				</table>
				
				<div class="prompt" style="box-sizing: border-box; color: #9e9e9e; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; margin-top: 20px;">
    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                邮件为系统自动发送
            </td>
        </tr>
        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
            <td class="aligncenter" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; text-align: center; vertical-align: top;">
                无需回复！
            </td>
        </tr>
    </table>
</div>
				
				
				<div class="footer" style="box-sizing: border-box; clear: both; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; padding: 20px; width: 100%;">
                    <table width="100%" style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                        <tr style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0;">
                            <td class="aligncenter content-block" style="box-sizing: border-box; color: #bdbdbd; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 12px; margin: 0; padding: 0 0 20px; text-align: center; vertical-align: top;">© Copyright 2011-@DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture) 17TRACK Rights Reserved</td>
                        </tr>
                    </table>
                </div>
				
			</div>
		</td>
		<td style="box-sizing: border-box; font-family: ''Helvetica Neue'', Helvetica, Arial, sans-serif; font-size: 14px; margin: 0; vertical-align: top;"></td>
	</tr>
</table>
</body>
</html>', 0, N'{
  "templateCode": 100061,
  "templateTypeId": "6599832728403968001",
  "titleDict": [
    {
      "id": "__0",
      "source": "Your offline transaction has been rejected",
      "target": "你的线下交易已经被驳回"
    }
  ],
  "bodyDict": [
    {
      "id": "__1",
      "source": "Your offline transaction has been rejected",
      "target": "你的线下交易已经被驳回"
    },
    {
      "id": "__2",
      "source": "Dear {0},",
      "target": "你好 {0}，"
    },
    {
      "id": "__3",
      "source": "We are sorry to inform you that your offline transaction has not been approved for the following reasons:",
      "target": "很遗憾地通知你，你的线下交易因为以下原因没有通过审核："
    },
    {
      "id": "__4",
      "source": "Click to view details",
      "target": "查看详情>>"
    },
    {
      "id": "__5",
      "source": "We dismissed it, please refer to the reason for the rejection and reapply.",
      "target": "我们将它驳回了，请参考驳回原因修改后重新上传。"
    },
    {
      "id": "__6",
      "source": "If you encounter problems, you can contact our customer service staff through QQ: {1} or email {2}, we will be happy to help you!",
      "target": "如果遇到问题可以通过QQ：{1} 或者邮箱 {2} 联系我们的客服人员，我们将竭诚为你服务！"
    },
    {
      "id": "__7",
      "source": "Best regards.",
      "target": ""
    },
    {
      "id": "__8",
      "source": "17TRACK Team",
      "target": "17TRACK 团队"
    },
    {
      "id": "__9",
      "source": "This is an automatically generated email and sent from a notification-only address.",
      "target": "邮件为系统自动发送"
    },
    {
      "id": "__10",
      "source": "Please do not reply to this message!",
      "target": "无需回复！"
    }
  ]
}')
GO
```
* 邮件服务重新发布(卫龙)