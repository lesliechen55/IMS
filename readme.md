# 集中式后台权限管理系统

## 后续

* 操作日志功能模块(利用methodaop+Mediator提升处理性能以及解耦, 不在拦截面过于逻辑)
* 权限相关修改触发缓存失效，可以采用事件分发，权限缓存key前缀Permission开始
* 通过swagger+webapi形成纯后端模式
* 追加miniprofile性能调用可视化
* 增加定时调度任务基础框架( IHostService hangfire quartz.net )

## 持续

代码无限整理+优化


```sql
Scaffold-DbContext 'Data Source=192.168.1.206;Initial Catalog=YQTrackV6_Pay2;User ID=sa;password=sa17track.net;' Microsoft.EntityFrameworkCore.SqlServer -UseDatabaseNames -Tables TBusinessType, TCurrency, TExchangeRate, TPayment, TPaymentLog, TProduct, TProductCategory, TProductSku, TProductSkuPrice, TProvider, TProviderType, TProviderTypeCurrency, TPurchaseOrder, TPurchaseOrderItem, TReconcile, TReconcileItem, TSequence, TSerialNo -Context PayDbContext -OutputDir Models
```
