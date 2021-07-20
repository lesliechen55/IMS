## 数据库变更文档

### 1、YQTrackV6_Pay2 库增加 TActivity表、TActivityCoupon表


### TActivity表 sql脚本 
```sql
USE [YQTrackV6_Pay2]
GO

/****** Object:  Table [dbo].[TActivityCoupon]    Script Date: 2021/6/21 15:02:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TActivityCoupon](
	[FActivityCouponId] [bigint] NOT NULL,
	[FActivityId] [bigint] NOT NULL,
	[FPurchaseOrderId] [bigint] NULL,
	[FActualDiscount] [decimal](18, 2) NULL,
	[FUserId] [bigint] NOT NULL,
	[FEmail] [varchar](256) NOT NULL,
	[FStartTime] [datetime] NOT NULL,
	[FEndTime] [datetime] NOT NULL,
	[FStatus] [tinyint] NOT NULL,
	[FRule] [varchar](100) NULL,
	[FSource] [varchar](50) NOT NULL,
	[FCreateBy] [bigint] NOT NULL,
	[FCreateAt] [datetime] NOT NULL,
	[FUpdateBy] [bigint] NOT NULL,
	[FUpdateAt] [datetime] NULL,
 CONSTRAINT [PK_TCoupon] PRIMARY KEY CLUSTERED 
(
	[FActivityCouponId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TActivityCoupon] ADD  CONSTRAINT [DF_TCoupon_FStatus]  DEFAULT ((1)) FOR [FStatus]
GO

ALTER TABLE [dbo].[TActivityCoupon] ADD  CONSTRAINT [DF_TCoupon_FCreateAt]  DEFAULT (getutcdate()) FOR [FCreateAt]
GO

ALTER TABLE [dbo].[TActivityCoupon] ADD  CONSTRAINT [DF_TCoupon_FUpdateAt]  DEFAULT (getutcdate()) FOR [FUpdateAt]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠券ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FActivityCouponId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FActivityId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单详情ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FPurchaseOrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FEmail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'享受的优惠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FRule'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FCreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FCreateAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUpdateAt'
GO
```

### TActivityCoupon表 sql脚本
```sql
USE [YQTrackV6_Pay2]
GO

/****** Object:  Table [dbo].[TActivity]    Script Date: 2021/6/21 15:02:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TActivity](
	[FActivityId] [bigint] IDENTITY(1,1) NOT NULL,
	[FCnName] [nvarchar](100) NOT NULL,
	[FEnName] [varchar](100) NOT NULL,
	[FDescription] [nvarchar](200) NULL,
	[FActivityType] [tinyint] NOT NULL,
	[FDiscountType] [tinyint] NOT NULL,
	[FCouponMode] [tinyint] NOT NULL,
	[FBusinessType] [int] NOT NULL,
	[FProductId] [bigint] NOT NULL,
	[FSkuCodes] [varchar](max) NULL,
	[FStatus] [tinyint] NOT NULL,
	[FRules] [varchar](max) NOT NULL,
	[FStartTime] [datetime] NOT NULL,
	[FEndTime] [datetime] NOT NULL,
	[FTerm] [int] NOT NULL,
	[FInternalUse] [bit] NOT NULL,
	[FCreateBy] [bigint] NOT NULL,
	[FCreateAt] [datetime] NOT NULL,
	[FUpdateBy] [bigint] NOT NULL,
	[FUpdateAt] [datetime] NOT NULL,
 CONSTRAINT [PK_TActivity] PRIMARY KEY CLUSTERED 
(
	[FActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TActivity_FDiscountMode]  DEFAULT ((0)) FOR [FCouponMode]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TActivity_FBusinessType]  DEFAULT ((0)) FOR [FBusinessType]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TDiscount_FStatus]  DEFAULT ((1)) FOR [FStatus]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TDiscount_FTerm]  DEFAULT ((0)) FOR [FTerm]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TDiscount_FCreateAt]  DEFAULT (getutcdate()) FOR [FCreateAt]
GO

ALTER TABLE [dbo].[TActivity] ADD  CONSTRAINT [DF_TDiscount_FUpdateAt]  DEFAULT (getutcdate()) FOR [FUpdateAt]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣活动ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FActivityId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动中文名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动英文名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FEnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FActivityType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'规则类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FDiscountType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FProductId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品Sku集合' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FSkuCodes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用规则' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FRules'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有效期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FTerm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否内部专用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FInternalUse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCreateAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FUpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FUpdateAt'
GO
```

