using Core.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebUI.Helpers
{
    public static class HtmlHelpers
    {
        public enum TextBoxType
        {
            PhoneNumber,
            Date,
            DateTime,
            TcNo,
            Sifre
        }

        public enum AlertType
        {
            Primary,
            Danger,
            Success,
            Warning,
            Info
        }

        public enum ButtonSize
        {
            sm,
            m,
            lg
        }

        public enum ButtonType
        {
            primary,
            danger,
            success
        }

        public static MvcHtmlString FormTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            bool disabled = false,
            string label = "",
            string classString = "",
            TextBoxType? textBoxType = null,
            bool? setRequired = null,
            bool forFilter = false,
            string addClass = "")
        {
            var isRequired = html.ViewData.Model.GetIsRequired(expression) || (setRequired != null && (bool)setRequired);

            var textBoxClass = $"form-control {addClass}";

            if (textBoxType == null && !forFilter && expression.ToString().ToLower().IndexOf("eposta") == -1)
            {
                textBoxClass = $"{textBoxClass}firstCapitalUpper";
            }

            if (textBoxType != null)
            {
                switch (textBoxType)
                {
                    case TextBoxType.PhoneNumber:
                        {
                            textBoxClass = $"{textBoxClass} phone-number";
                            break;
                        }
                    case TextBoxType.Date:
                        {
                            textBoxClass = $"{textBoxClass} pickadate";
                            break;
                        }
                    case TextBoxType.DateTime:
                        {
                            textBoxClass = $"{textBoxClass} pickadatetime";
                            break;
                        }
                    case TextBoxType.TcNo:
                        {
                            textBoxClass = $"{textBoxClass} tcNo";
                            break;
                        }
                }
            }

            if (!string.IsNullOrEmpty(classString))
            {
                textBoxClass = $"{textBoxClass} {classString}";
            }

            var htmlString = string.Empty;

            object htmlAttributes = null;

            var placeHolder = string.IsNullOrEmpty(label)
                ? html.ViewData.Model.GetDisplayName(expression)
                : label;

            if (disabled)
            {
                htmlAttributes = new { @class = textBoxClass, @placeholder = placeHolder, @autocomplete = "off", disabled = "disabled" };
            }
            else
            {
                htmlAttributes = new { @class = textBoxClass, @placeholder = placeHolder, @autocomplete = "off" };
            }

            if (!forFilter)
            {
                if (textBoxType == TextBoxType.Sifre)
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression, label)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.PasswordFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                    }
                    else
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.PasswordFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                    }
                }
                else if (textBoxType == TextBoxType.TcNo)
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression, label)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.TextBoxFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div><div class=\"row mt-0\"><div class=\"col-lg-12\"><div class=\"alert alert-info\" id=\"div{html.IdFor(expression)}\"><span class=\"font-weight-semibold\">{Resources.LangResources.TcKimlikKontrolEdebilmekIcin}</span></div></div></div>";
                    }
                    else
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.TextBoxFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div><div class=\"row mt-0\"><div class=\"col-lg-12\"><div class=\"alert alert-info\" id=\"div{html.IdFor(expression)}\"><span class=\"font-weight-semibold\">{Resources.LangResources.TcKimlikKontrolEdebilmekIcin}</span></div></div></div>";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression, label)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.TextBoxFor(expression, (textBoxType == TextBoxType.Date ? AyarlarService.Get().GecerliTarihFormatiString : textBoxType == TextBoxType.DateTime ? AyarlarService.Get().GecerliTarihSaatFormatiString : ""), htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                    }
                    else
                    {
                        htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.TextBoxFor(expression, (textBoxType == TextBoxType.Date ? AyarlarService.Get().GecerliTarihFormatiString : textBoxType == TextBoxType.DateTime ? AyarlarService.Get().GecerliTarihSaatFormatiString : ""), htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                    }
                }
            }
            else
            {
                htmlString = html.TextBoxFor(expression, (textBoxType == TextBoxType.Date ? AyarlarService.Get().GecerliTarihFormatiString : textBoxType == TextBoxType.DateTime ? AyarlarService.Get().GecerliTarihSaatFormatiString : ""), htmlAttributes).ToHtmlString();
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormCellTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            bool disabled = false,
            string label = "",
            string classString = "",
            TextBoxType? textBoxType = null,
            bool? setRequired = null,
            bool forFilter = false,
            string addClass = "")
        {
            var isRequired = html.ViewData.Model.GetIsRequired(expression) || (setRequired != null && (bool)setRequired);

            var textBoxClass = $"form-control {addClass}";

            if (textBoxType == null && !forFilter && expression.ToString().ToLower().IndexOf("eposta") == -1)
            {
                textBoxClass = $"{textBoxClass}firstCapitalUpper";
            }

            if (textBoxType != null)
            {
                switch (textBoxType)
                {
                    case TextBoxType.PhoneNumber:
                        {
                            textBoxClass = $"{textBoxClass} phone-number";
                            break;
                        }
                    case TextBoxType.Date:
                        {
                            textBoxClass = $"{textBoxClass} pickadate";
                            break;
                        }
                    case TextBoxType.DateTime:
                        {
                            textBoxClass = $"{textBoxClass} pickadatetime";
                            break;
                        }
                    case TextBoxType.TcNo:
                        {
                            textBoxClass = $"{textBoxClass} tcNo";
                            break;
                        }
                }
            }

            if (!string.IsNullOrEmpty(classString))
            {
                textBoxClass = $"{textBoxClass} {classString}";
            }

            var htmlString = string.Empty;

            object htmlAttributes = null;

            var placeHolder = string.IsNullOrEmpty(label)
                ? html.ViewData.Model.GetDisplayName(expression)
                : label;

            if (disabled)
            {
                htmlAttributes = new { @class = textBoxClass, @placeholder = placeHolder, @autocomplete = "off", disabled = "disabled" };
            }
            else
            {
                htmlAttributes = new { @class = textBoxClass, @placeholder = placeHolder, @autocomplete = "off" };
            }

            if (!forFilter)
            {
                if (textBoxType == TextBoxType.Sifre)
                {
                    htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-5\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-7\">{html.PasswordFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                }
                else if (textBoxType == TextBoxType.TcNo)
                {
                    htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-5\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-7\">{html.TextBoxFor(expression, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div><div class=\"row mt-0\"><div class=\"col-lg-12\"><div class=\"alert alert-info\" id=\"div{html.IdFor(expression)}\"><span class=\"font-weight-semibold\">{Resources.LangResources.TcKimlikKontrolEdebilmekIcin}</span></div></div></div>";
                }
                else
                {
                    htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-5\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-7\">{html.TextBoxFor(expression, (textBoxType == TextBoxType.Date ? AyarlarService.Get().GecerliTarihFormatiString : textBoxType == TextBoxType.DateTime ? AyarlarService.Get().GecerliTarihSaatFormatiString : ""), htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                }
            }
            else
            {
                htmlString = html.TextBoxFor(expression, (textBoxType == TextBoxType.Date ? AyarlarService.Get().GecerliTarihFormatiString : textBoxType == TextBoxType.DateTime ? AyarlarService.Get().GecerliTarihSaatFormatiString : ""), htmlAttributes).ToHtmlString();
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormTextAreaFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            int rows,
            int cols,
            bool? setRequired = null)
        {
            var isRequired = html.ViewData.Model.GetIsRequired(expression) || (setRequired != null && (bool)setRequired);

            var htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.TextAreaFor(expression, new { @class = "form-control", rows, cols, @placeholder = html.ViewData.Model.GetDisplayName(expression) })}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormFileInputFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            string name,
            bool multiple = false,
            bool? setRequired = null)
        {
            var isRequired = html.ViewData.Model.GetIsRequired(expression) || (setRequired != null && (bool)setRequired);

            var htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)} {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\"><input type =\"file\" name=\"{name}\" class=\"form-control-uniform\" " + (multiple ? "multiple=\"multiple\"" : "") + "/></div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormFileShowDeleteFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            bool showIt,
            string imageSrc,
            string name,
            string value,
            string description)
        {
            var htmlString = string.Empty;

            if (showIt)
            {
                htmlString = $"{html.HiddenFor(expression)}<div class=\"form-group row\"><div class=\"col-lg-10 offset-lg-2\"><img src=\"{imageSrc}\"/></div></div><div class=\"form-group row\"><div class=\"col-lg-10 offset-lg-2\"><button type=\"submit\" class=\"btn btn-danger btn-sm\" name=\"{name}\" value=\"{value}\" ><i class=\"icon-cross mr-2\"></i>{description}</button></div></div>";
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormCheckBoxFor<TModel>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, bool>> expression)
        {
            return MvcHtmlString.Create($"<div class=\"form-group row\"><div class=\"col-lg-10 offset-lg-2\"><label>{html.CheckBoxFor(expression, new { @class = "icheck" })}{html.LabelFor(expression)}</label></div></div>");
        }

        public static MvcHtmlString FormCheckBoxOnly<TModel>(
           this HtmlHelper<TModel> html,
           Expression<Func<TModel, bool>> expression,
           string label,
           string addClass = "")
        {
            return MvcHtmlString.Create($"<label>{html.CheckBoxFor(expression, new { @class = $"icheck {addClass}" })}{label}</label>");
        }

        public static MvcHtmlString FormRadioButtonBooleanFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            string trueDescription,
            string falseDescription)
        {
            return MvcHtmlString.Create($"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{html.LabelFor(expression)}</label><div class=\"col-lg-10\"><label style=\"margin-right:30px;\">{html.RadioButtonFor(expression, true, new { @class = "icheck" })}{trueDescription}</label><label>{html.RadioButtonFor(expression, false, new { @class = "icheck" })}{falseDescription}</label></div></div>");
        }

        public static MvcHtmlString FormDropDownListSelectableFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            List<SelectListItem> selectListItems,
            bool showIt,
            string description,
            string onchangeUrl,
            string optionLabel = "")
        {
            var selectItem = new SelectListItem
            {
                Selected = selectListItems.Count(x => x.Selected) <= 0,
                Text = string.IsNullOrEmpty(optionLabel) ? Resources.LangResources.Seciniz : optionLabel,
                Value = "0"
            };

            selectListItems.Insert(0, selectItem);

            var htmlString = string.Empty;

            if (showIt)
            {
                object htmlAttributes;

                if (string.IsNullOrEmpty(onchangeUrl))
                {
                    htmlAttributes = new { @class = "form-control select2" };
                }
                else
                {
                    htmlAttributes = new { @class = "form-control select2", @onchange = $"OnChangeToUpdate('{onchangeUrl}' + this.value)" };
                }

                htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{description}</label><div class=\"col-lg-10\">{html.DropDownListFor(expression, selectListItems, htmlAttributes)}</div></div>";
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            List<SelectListItem> selectListItems,
            bool multiSelect = false,
            string onchangeUrl = "",
            bool? setRequired = null,
            string optionLabel = "",
            bool disabled = false,
            bool forFilter = false,
            string addClass = "",
            string labelValue = ""
            )
        {
            var isRequired = html.ViewData.Model.GetIsRequired(expression) || (setRequired != null && (bool)setRequired);

            if (selectListItems == null) selectListItems = new List<SelectListItem>();

            var htmlString = string.Empty;

            object htmlAttributes = null;
            var select2 = multiSelect ? "select2-multiple" : "select2";

            var onChange = string.Empty;

            if (disabled)
            {
                if (!string.IsNullOrEmpty(onchangeUrl))
                {
                    htmlAttributes = new { @class = $"form-control {select2} {addClass}", disabled = "disabled", @onchange = $"OnChangeToUpdate('{onchangeUrl}')" };

                }
                else
                {
                    htmlAttributes = new { @class = $"form-control {select2} {addClass}", disabled = "disabled" };
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(onchangeUrl))
                {
                    htmlAttributes = new { @class = $"form-control {select2} {addClass}", @onchange = $"OnChangeToUpdate('{onchangeUrl}')" };
                }
                else
                {
                    htmlAttributes = new { @class = $"form-control {select2} {addClass}" };
                }
            }



            if (!string.IsNullOrEmpty(optionLabel) && forFilter)
            {
                // if (selectListItems.Count == 0)
                // {
                var selectItem = new SelectListItem
                {
                    Selected = true,
                    Text = optionLabel,
                    Value = "0"
                };

                selectListItems.Insert(0, selectItem);
                //}
                // else
                // {
                //     selectListItems[0].Text = optionLabel;
                //     selectListItems[0].Value = "0";
                // }
            }

            var label = !string.IsNullOrEmpty(labelValue)
                ? $"<label>{labelValue}</label>"
                : html.LabelFor(expression).ToString();

            if (multiSelect)
            {
                var listString = html.ListBoxFor(expression, selectListItems, htmlAttributes);

                if (!forFilter)
                {
                    htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{label}  {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{listString}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label" })}</div></div>";
                }
                else
                {
                    htmlString = listString.ToHtmlString();
                }
            }
            else
            {
                if (!forFilter)
                {
                    htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{label}  {html.Raw(AttributeHelpers.GetHtmlIfIsRequired(isRequired))}</label><div class=\"col-lg-10\">{html.DropDownListFor(expression, selectListItems, Resources.LangResources.Seciniz, htmlAttributes)}{html.ValidationMessageFor(expression, "", new { @class = "validation-invalid-label", style = "" })}</div></div>";
                }
                else
                {
                    htmlString = html.DropDownListFor(expression, selectListItems, htmlAttributes).ToHtmlString();
                }
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormButtons<TModel>(
            this HtmlHelper<TModel> html,
            int id,
            string divModalDelete,
            bool deletable = false,
            bool cancelbutton = true,
            string command = "")
        {
            var commandValue = !string.IsNullOrEmpty(command) ? command : "Duzenle";

            var htmlString = $"<div class=\"form-group row\"><div class=\"col-lg-10 offset-lg-2\"><button type =\"submit\" class=\"btn btn-outline-primary btn-m\" name =\"Command\" value =\"{commandValue}\">{Resources.LangResources.KAYDET}</button>";

            if (id > 0 && deletable)
            {
                htmlString = $"{htmlString}&nbsp;<button type =\"button\" class=\"btn btn-outline-danger btn-m\" data-toggle=\"modal\" data-target=\"#{divModalDelete}\">{Resources.LangResources.SIL}</button>";
            }

            if (cancelbutton)
            {
                htmlString = $"{htmlString}&nbsp;<button type =\"submit\" class=\"btn btn-outline-info btn-m cancel\" name =\"Command\" value =\"Iptal\">{Resources.LangResources.IPTAL}</button></div></div>";
            }

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormSubmit<TModel>(
            this HtmlHelper<TModel> html,
            ButtonSize buttonSize,
            ButtonType buttonType,
            string buttonId,
            string buttonText)
        {
            var htmlString = $"<button id=\"{buttonId}\" type =\"submit\" name =\"Command\" class=\"btn btn-outline-{buttonType.ToString()} btn-{buttonSize.ToString()}\">{buttonText}</button>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormButton<TModel>(
            this HtmlHelper<TModel> html,
            ButtonSize buttonSize,
            ButtonType buttonType,
            string buttonId,
            string buttonText)
        {
            var htmlString = $"<button id=\"{buttonId}\" type =\"button\" class=\"btn btn-outline-{buttonType.ToString()} btn-{buttonSize.ToString()}\">{buttonText}</button>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString LinkButton<TModel>(
            this HtmlHelper<TModel> html,
            ButtonSize buttonSize,
            ButtonType buttonType,
            string buttonId,
            string buttonText,
            string url)
        {
            var htmlString = $"<a id=\"{buttonId}\" type =\"button\" class=\"btn btn-outline-{buttonType.ToString()} btn-{buttonSize.ToString()}\" href=\"{url}\">{buttonText}</button>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormDataTablesFor<TModel>(
            this HtmlHelper<TModel> html,
            string title,
            string buttonText,
            string editUrl,
            List<MvcHtmlString> filters = null)
        {
            var htmlString = $"<div class=\"card\"><div class=\"card-header header-elements-inline\"><h5 class=\"card-title\">{title}</h5>{(!string.IsNullOrEmpty(editUrl) ? $"<div class=\"header-elements\"><button type=\"button\" class=\"btn btn-primary\" onclick=\"location.href ='{editUrl}'\">{buttonText}<i class=\"icon-plus2 ml-2\"></i></button></div>" : string.Empty)}</div><div class=\"card-body\"><fieldset class=\"mb-3\"><legend class=\"text-uppercase font-size-sm font-weight-bold\" ></legend>";

            var htmlFilter = string.Empty;

            if (filters != null)
            {
                htmlFilter = $"{htmlFilter}<div class=\"form-inline\" id=\"divFilters\">";

                foreach (var filter in filters)
                {
                    htmlFilter = $"{htmlFilter}<div class=\"form-group margin-top-10\">";

                    htmlFilter = $"{htmlFilter}{filter}";

                    htmlFilter = $"{htmlFilter}</div>";
                }

                htmlFilter = $"{htmlFilter}</div>";
            }

            htmlString = $"{htmlString}{htmlFilter}<table id=\"tblListele\" class=\"table datatable-show-all dtr-inline\"><thead><tr></tr></thead></table></fieldset></div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormCardDivBegin<TModel>(this HtmlHelper<TModel> html, string title)
        {
            var htmlString = $"<div class=\"card\"><div class=\"card-header header-elements-inline\"><h5 class=\"card-title\">{title}</h5></div><div class=\"card-body\"><fieldset class=\"mb-3\"><legend class=\"text-uppercase font-size-sm font-weight-bold\"></legend>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormCardDivEnd<TModel>(this HtmlHelper<TModel> html)
        {
            var htmlString = "</fieldset></div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormGroupRowBegin<TModel>(this HtmlHelper<TModel> html, string title)
        {
            var htmlString = $"<div class=\"form-group row\"><label class=\"col-form-label col-lg-2\">{title}</label><div class=\"col-lg-10\">";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormGroupRowEnd<TModel>(this HtmlHelper<TModel> html)
        {
            var htmlString = $"</div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormScrollableTableThead<TModel>(this HtmlHelper<TModel> html, List<string> theads, string tableId = "")
        {
            var idTag = !string.IsNullOrEmpty(tableId) ? $" id=\"{tableId}\"" : "";

            var htmlString = $"<div class=\"table-responsive scarea\" style=\"height: 410px;\"><table class=\"table table-striped table-hover table-condensed table-bordered\" {idTag}><thead><tr>";

            foreach (var thead in theads)
            {
                htmlString = $"{htmlString}<th>{thead}</th>";
            }

            htmlString = $"{htmlString}</tr></thead><tbody>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormScrollableTableRow<TModel>(this HtmlHelper<TModel> html, List<object> tbodies, string style = "", string css = "")
        {
            var htmlString = $"<tr {(!string.IsNullOrEmpty(style) ? $"style=\"{style}\"" : "")} {(!string.IsNullOrEmpty(css) ? $"class=\"{css}\"" : "")}>";

            foreach (var td in tbodies)
            {
                htmlString = $"{htmlString}<td>{td}</td>";
            }

            htmlString = $"{htmlString}</tr>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormScrollableTableEnd<TModel>(this HtmlHelper<TModel> html)
        {
            var htmlString = "</tbody></table></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormMessagesFor<TModel>(
            this HtmlHelper<TModel> html,
            AlertType alertType,
            string message)
        {
            var htmlClass = "";

            switch (alertType)
            {
                case AlertType.Primary:
                    {
                        htmlClass = "alert-primary";
                        break;
                    }
                case AlertType.Danger:
                    {
                        htmlClass = "alert-danger";
                        break;
                    }
                case AlertType.Success:
                    {
                        htmlClass = "alert-success";
                        break;
                    }
                case AlertType.Warning:
                    {
                        htmlClass = "alert-warning";
                        break;
                    }
                case AlertType.Info:
                    {
                        htmlClass = "alert-info";
                        break;
                    }
            }

            var htmlString = $"<div class=\"row mt-0\"><div class=\"col-lg-10 offset-2\"><div class=\"alert {htmlClass} alert-dismissible\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span>×</span></button><span class=\"font-weight-semibold\">{message}</span></div></div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormDeletePopupMessageFor<TModel>(
            this HtmlHelper<TModel> html,
            string divModalDelete,
            string textTitle,
            string text)
        {
            var title = Resources.LangResources.DIKKAT;
            var closeButton = Resources.LangResources.KAPAT;
            var submitButton = Resources.LangResources.ONAYLA;

            var htmlString = $"<div id=\"{divModalDelete}\" class=\"modal fade\" tabindex=\"-1\"><div class=\"modal-dialog\"><div class=\"modal-content\"><div class=\"modal-header bg-danger\"><h6 class=\"modal-title\">{title}</h6><button type =\"button\" class=\"close\" data-dismiss=\"modal\">×</button></div><div class=\"modal-body\">{(!string.IsNullOrEmpty(textTitle) ? $"<h6 class=\"font-weight-semibold\">{textTitle}</h6>" : string.Empty)}{text}</div><div class=\"modal-footer\"><button type=\"button\" class=\"btn bg-info btn-m\" data-dismiss=\"modal\">{closeButton}</button><button type=\"submit\" class=\"btn bg-danger btn-m\" >{submitButton}</button></div></div></div></div>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString FormSpanBadge<TModel>(this HtmlHelper<TModel> html, bool isOk, string info)
        {
            var htmlString = $"<span class=\"badge badge-{(isOk ? "success" : "danger")}\">{info}</span>";

            return MvcHtmlString.Create(htmlString);
        }

        public static MvcHtmlString PartialFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            string partialName)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string modelName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            object model = metaData.Model;

            if (partialName == null)
            {
                partialName = metaData.TemplateHint == null
                    ? typeof(TProperty).Name
                    : metaData.TemplateHint;
            }

            ViewDataDictionary viewData = new ViewDataDictionary(html.ViewData)
            {
                TemplateInfo = new TemplateInfo { HtmlFieldPrefix = modelName }
            };

            return html.Partial(partialName, model, viewData);
        }
    }
}