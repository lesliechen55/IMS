using FluentValidation;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class InvoiceEditRequestValidator : AbstractValidator<InvoiceEditRequest>
    {
        public InvoiceEditRequestValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty();
            RuleFor(x => x.UserEmail).NotEmpty().MaximumLength(256);
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.TaxNo).NotEmpty().MaximumLength(32);
            RuleFor(x => x.InvoiceType).IsInEnum().NotEqual(InvoiceType.None);
            RuleFor(x => x.ExpressAddress).NotEmpty().MaximumLength(256);
            RuleFor(x => x.InvoiceEmail).NotEmpty().MaximumLength(256);
            RuleFor(x => x.Contact).NotEmpty().MaximumLength(16);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(32);
            RuleFor(x => x.TaxPayerCertificateUrl).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InvoiceAddRequest request)
                {
                    if (request.InvoiceType == InvoiceType.Special && x.IsNullOrWhiteSpace())
                    {
                        y.AddFailure($"一般纳税人证明必须上传，增值专用发票必填项");
                    }
                }
            });
            RuleFor(x => x.Bank).MaximumLength(32).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InvoiceAddRequest request)
                {
                    if (request.InvoiceType == InvoiceType.Special && x.IsNullOrEmpty())
                    {
                        y.AddFailure($"{nameof(request.Bank)}参数不能为空，增值专用发票必填项");
                    }
                }
            });
            RuleFor(x => x.BankAccount).MaximumLength(32).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InvoiceAddRequest request)
                {
                    if (request.InvoiceType == InvoiceType.Special && x.IsNullOrEmpty())
                    {
                        y.AddFailure($"{nameof(request.BankAccount)}参数不能为空，增值专用发票必填项");
                    }
                }
            });
            RuleFor(x => x.Address).MaximumLength(256).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InvoiceAddRequest request)
                {
                    if (request.InvoiceType == InvoiceType.Special && x.IsNullOrEmpty())
                    {
                        y.AddFailure($"{nameof(request.Address)}参数不能为空，增值专用发票必填项");
                    }
                }
            });
            RuleFor(x => x.Telephone).MaximumLength(32).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InvoiceAddRequest request)
                {
                    if (request.InvoiceType == InvoiceType.Special && x.IsNullOrEmpty())
                    {
                        y.AddFailure($"{nameof(request.Telephone)}参数不能为空，增值专用发票必填项");
                    }
                }
            });
        }
    }
}