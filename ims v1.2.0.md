## IMS v1.2.0迭代发布文档

> 逐步整合卖家额度、店铺、同步情况等相关信息，便于客服快速反馈用户的问题的咨询

### 发布需要调整项

* 新增三个权限：系统中心-->业务系统-->注册用户查询 节点下面加子权限：用户店铺、大批量任务列表、店铺导入记录
* 新增Appsetting有关Seller数据库配置节点：SellerOrderDBContext、SellerMessageDBContext

```json
"SellerOrderDBContext": "Data Source=172.16.1.246,4433;Initial Catalog=YQTrackV5_SellerOrder1;User ID=17userdbvisitor;password=ZtQJe3WXbJkCJo4Z;persist security info=true;",

"SellerMessageDBContext": "Data Source=172.16.1.246,4433;Initial Catalog=YQTrackV5_SellerMessage1;User ID=17userdbvisitor;password=ZtQJe3WXbJkCJo4Z;persist security info=true;"
```

### 需要协作发布的项目

* YQTrack.Backend.Release 分支feature/standardLanguageHelperUpdate 合并到master分支

### 迭代修改项

1. 会员详情新增Seller跟踪额度、邮件额度展示。
2. 新增Seller用户店铺列表、搜索、分页等。
3. 新增Seller用户大批量操作的任务列表、搜索、分页等。
4. 新增Seller用户店铺导入历史信息（默认15条）。


