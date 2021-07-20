## 数据库变更文档

### 1、Message数据库：
- TSendTask 新加CreateBy(nvarchar(50))，UpdateBy(nvarchar(50))字段，用来记录创建人和最后修改人。

```sql
ALTER TABLE [dbo].[TSendTask] 
ADD FCreateBy NVARCHAR(50) NULL, FUpdateBy NVARCHAR(50) NULL
```

### 2、PayDb数据库：
1. TProviderTypeCurrency 添加联合主键( 无用 )
2. TSerialNo 增加主键
3. TProductCategory TProduct 主键自增 + 增加FCode唯一约束
4. TProduct 增加唯一索引：UX_TProduct_FName
5. TProductSku 增加唯一索引：UX_TProductSku_FName

### 3. IMSAdmin数据:

* 移除 IX_TPermission_FName 唯一索引
* 增加 FEmail 字段 TManager 表

```sql
DROP INDEX TPermission.IX_TPermission_FName

ALTER TABLE [dbo].[TManager] 
ADD FEmail NVARCHAR(128) NULL
```


### PayDb数据库备注Sql变更脚本整理

```sql
--TSerialNo 增加主键
ALTER TABLE [dbo].[TSerialNo]
ADD PRIMARY KEY ([FType])

--TProductCategory 主键自增 + 增加FCode唯一约束
ALTER TABLE [dbo].[TProductCategory]
ADD FProductCategoryId INT IDENTITY

alter table [dbo].[TProductCategory] 
add constraint [UX_TCategoryCode] unique([FCode])

-- TProduct 主键自增 + 增加FCode唯一约束
ALTER TABLE [dbo].[TProduct]
ADD FProductId INT IDENTITY

alter table [dbo].[TProduct] 
add constraint [UX_TCode] unique([FCode])

-- TProduct 增加唯一索引：UX_TProduct_FName
alter table [dbo].[TProduct] 
add constraint [UX_TProduct_FName] unique([FName])

-- TProductSku 增加唯一索引：UX_TProductSku_FName
alter table [dbo].[TProductSku] 
add constraint [UX_TProductSku_FName] unique([FName])
```