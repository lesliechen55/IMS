## IMS v1.3.0迭代发布文档

> 目前Kibana在操作上需要有一定的开发背景，产品/客服/其他人员查看比较困难。将部分常规分析的内容到IMS，方便相关人员统一查询

### 需要新增的数据库/表

- 数据库：YQTrackV6_Admin
  - 数据表：TPermission增加一个字段：FIsMultiAction
    ```sql
     ALTER TABLE [dbo].[TPermission]
     ADD [FIsMultiAction] BIT
    ```

  - 新增两张表：TESDashboard、TESField
    ```sql
    CREATE TABLE [dbo].[TESDashboard](
	    [FPermissionId] [int] NOT NULL,
	    [FDashboardSrc] [nvarchar](max) NOT NULL,
	    [FMaxDateRange] [int] NULL,
	    [FUsername] [nvarchar](50) NULL,
	    [FPassword] [nvarchar](50) NULL,
	    [FFieldsConfig] [nvarchar](max) NULL,
     CONSTRAINT [PK_TESDashboard] PRIMARY KEY CLUSTERED 
    (
	    [FPermissionId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO

    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大可选日期范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TESDashboard', @level2type=N'COLUMN',@level2name=N'FMaxDateRange'
    GO

    CREATE TABLE [dbo].[TESField](
	    [FId] [int] IDENTITY(1,1) NOT NULL,
	    [FName] [nvarchar](50) NOT NULL,
	    [FValue] [nvarchar](50) NULL,
	    [FCategory] [nvarchar](50) NOT NULL,
	    [FSort] [int] NULL,
     CONSTRAINT [PK_TESField] PRIMARY KEY CLUSTERED 
    (
	    [FId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET IDENTITY_INSERT [dbo].[TESField] ON 
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (11, N'Last 15 minutes', N'from:now-15m,mode:quick,to:now', N'TimeRange', 0)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (12, N'Last 30 minutes', N'from:now-30m,mode:quick,to:now', N'TimeRange', 1)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (13, N'Last 1 hour', N'from:now-1h,mode:quick,to:now', N'TimeRange', 2)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (14, N'Last 2 hour', N'from:now-2h,mode:quick,to:now', N'TimeRange', 3)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (16, N'true', N'true', N'Success', 0)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (17, N'false', N'false', N'Success', 1)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (25, N'Last 4 hour', N'from:now-4h,mode:quick,to:now', N'TimeRange', 4)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (26, N'Last 12 hour', N'from:now-12h,mode:quick,to:now', N'TimeRange', 5)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (28, N'Today', N'from:now/d,mode:quick,to:now/d', N'TimeRange', 6)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (30, N'Last 24 hour', N'from:now-24h,mode:quick,to:now', N'TimeRange', 7)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (31, N'Yesterday', N'from:now-1d/d,mode:quick,to:now-1d/d', N'TimeRange', 8)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (32, N'Day before yesterday', N'from:now-2d/d,mode:quick,to:now-2d/d', N'TimeRange', 9)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (33, N'This week', N'from:now/w,mode:quick,to:now/w', N'TimeRange', 10)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (34, N'This day last week', N'from:now-7d/d,mode:quick,to:now-7d/d', N'TimeRange', 11)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (35, N'Last 7 days', N'from:now-7d,mode:quick,to:now', N'TimeRange', 12)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (36, N'Previous week', N'from:now-1w/w,mode:quick,to:now-1w/w', N'TimeRange', 13)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (37, N'This month', N'from:now/M,mode:quick,to:now/M', N'TimeRange', 14)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (38, N'Last 30 days', N'from:now-30d,mode:quick,to:now', N'TimeRange', 15)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (39, N'Previous month', N'from:now-1M/M,mode:quick,to:now-1M/M', N'TimeRange', 16)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (40, N'Last 60 days', N'from:now-60d,mode:quick,to:now', N'TimeRange', 17)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (43, N'Last 90 days', N'from:now-90d,mode:quick,to:now', N'TimeRange', 18)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (44, N'Last 6 months', N'from:now-6M,mode:quick,to:now', N'TimeRange', 19)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (46, N'This year', N'from:now/y,mode:quick,to:now/y', N'TimeRange', 20)
    GO
    INSERT [dbo].[TESField] ([FId], [FName], [FValue], [FCategory], [FSort]) VALUES (47, N'Last 1 year', N'from:now-1y,mode:quick,to:now', N'TimeRange', 21)
    GO
    SET IDENTITY_INSERT [dbo].[TESField] OFF
    GO
    ```

### 发布需要调整项

* 新增两个权限：系统中心-->系统管理-->权限管理 节点下面加子权限：配置Dashboard、查看Dashboard
* 新增Appsetting配置节点：Kibana

### 需要协作发布的项目

无

### 迭代修改项

1. 修改：权限认证过滤器，支持根据传入的不同权限参数过滤权限。
2. 新增：配置Dashboard页面，并根据输入的Dashboard链接自动解析ES字段。
3. 新增：配置Dashboard数据保存与验证。
4. 新增：根据Dashboard配置进行筛选字段渲染，根据筛选字段值进行Dashboard展示。
5. 新增：是否枚举值选项区分用枚举名称或值搜索、快速日期范围最大范围设置。

