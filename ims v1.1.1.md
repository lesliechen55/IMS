## IMS v1.1.1迭代发布文档

> 将现有账号注销功能集成到IMS系统，客户人员可以直接根据情况进行操作，避免人工操作。业务模块调整，部分页面内容优化等。

### 发布需要调整项

* API用户中心的用户模块菜单调整：API用户管理-->API用户列表
* 删除交易里面里面的“人工对账”菜单
```sql
  UPDATE [YQTrackV6_Admin].[dbo].[TPermission] SET [FIsDeleted]=1 WHERE FFullName='Pay_ManualReconcile_Index' OR  FFullName='Pay_ManualReconcile_ImportGlocash'
```

* 系统中心里面的业务模块调整：注册用户查询、反馈信息管理、注册用户统计、注销用户查询
* 注册用户查询新增子权限：注销用户

### 需要协作发布的项目

* YQTrack.Backend.Release 分支feature/deleteUser合并到master分支
* YQTrack.Backend.Public 删除用户RPC服务端

### 迭代修改项

 1. 新增：注销用户功能（目前只支持买家、卖家、运输商）。
 2. 修改：反馈回复增加快捷回复操作。
 3. 修改：用户详情页调整。
 4. 修改：修正线下交易的弹出框的title。
 5. 新增：注销用户查询功能。
 6. 修改：部分菜单名称调整。
 7. 优化：部分页面样式、展示布局等优化。


