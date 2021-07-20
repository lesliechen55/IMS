## ���ݿ����ĵ�

### 1��YQTrackV6_Pay2 ������ TActivity��TActivityCoupon��


### TActivity�� sql�ű� 
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ż�ȯID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FActivityCouponId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ŻݻID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FActivityId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FPurchaseOrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUserId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�û�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FEmail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ܵ��Ż�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FRule'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Դ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FCreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FCreateAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivityCoupon', @level2type=N'COLUMN',@level2name=N'FUpdateAt'
GO
```

### TActivityCoupon�� sql�ű�
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ۿۻID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FActivityId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ӣ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FEnName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʹ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ۿ�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FActivityType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FDiscountType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FProductId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ƷSku����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FSkuCodes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'״̬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʹ�ù���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FRules'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ʼʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FStartTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FEndTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ч��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FTerm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ��ڲ�ר��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FInternalUse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCreateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FCreateAt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FUpdateBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�޸�ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TActivity', @level2type=N'COLUMN',@level2name=N'FUpdateAt'
GO
```

