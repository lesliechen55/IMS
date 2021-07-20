using FluentValidation;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class TransactionQueryRequestValidator : AbstractValidator<TransactionQueryRequest>
    {
        public TransactionQueryRequestValidator()
        {
            RuleFor(x => x.PaymentProvider).IsInEnum().Must(x => x.ValidateImsIgnore()).WithMessage(x => $"{nameof(x.PaymentProvider)}参数值错误");

            RuleFor(x => x.OrderId).Custom((x, y) =>
            {
                if (y.InstanceToValidate is TransactionQueryRequest request)
                {
                    if (request.TradeNo.IsNullOrWhiteSpace() && (!x.HasValue || x.Value == 0))
                    {
                        y.AddFailure($"{y.DisplayName}参数错误不能为空,缺少必要查询参数orderId或者tradeNo");
                    }
                }
            });

            RuleFor(x => x.TradeNo).Custom((x, y) =>
            {
                if (y.InstanceToValidate is TransactionQueryRequest request)
                {
                    if ((!request.OrderId.HasValue || request.OrderId == 0) && x.IsNullOrWhiteSpace())
                    {
                        y.AddFailure($"{y.DisplayName}参数错误不能为空,缺少必要查询参数orderId或者tradeNo");
                    }
                }
            });
        }
    }
}