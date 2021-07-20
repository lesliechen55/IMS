using FluentValidation;
using FluentValidation.Validators;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class PermissionEditRequestValidator : AbstractValidator<PermissionEditRequest>
    {
        public PermissionEditRequestValidator()
        {
            RuleFor(x => x.ParentId).Custom((x, y) =>
            {
                if (x.HasValue)
                {
                    if (x.Value <= 0)
                    {
                        y.AddFailure($"父级菜单参数错误,parentId不能小于等于0");
                    }
                }
            }).NotEqual(x => x.Id).WithMessage(x => $"您选择的父级菜单({x.ParentId})不能是自身编号({x.Id})");

            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().Length(4, 32);
            RuleFor(x => x.AreaName).MaximumLength(16);

            void Check(string x, CustomContext y)
            {
                if (y.InstanceToValidate is PermissionEditRequest request)
                {
                    if (request.MenuType.HasValue && request.MenuType.Value == MenuType.Function)
                    {
                        if (x.IsNullOrWhiteSpace())
                        {
                            y.AddFailure($"{y.DisplayName}参数错误不能为空,功能菜单此为必填项");
                        }
                    }
                }
            }

            RuleFor(x => x.ControllerName).Custom(Check).MaximumLength(32);
            RuleFor(x => x.ActionName).Custom(Check).MaximumLength(32);
            RuleFor(x => x.FullName).Custom(Check).MaximumLength(64);
            RuleFor(x => x.Url).Custom(Check).MaximumLength(128);

            RuleFor(x => x.Remark).NotEmpty().Length(4, 64);

            RuleFor(x => x.Icon).MaximumLength(64);

            RuleFor(x => x.Sort).GreaterThanOrEqualTo(0);

            RuleFor(x => x.MenuType).NotNull();

            RuleFor(x => x.TopMenuKey).Custom((x, y) =>
            {
                if (y.InstanceToValidate is PermissionEditRequest request)
                {
                    if (request.MenuType.HasValue && request.MenuType.Value == MenuType.TopMenu)
                    {
                        if (x.IsNullOrWhiteSpace())
                        {
                            y.AddFailure($"{y.DisplayName}参数错误不能为空,选择顶级菜单时此为必填项");
                        }
                    }
                }
            }).MaximumLength(32);
        }
    }
}