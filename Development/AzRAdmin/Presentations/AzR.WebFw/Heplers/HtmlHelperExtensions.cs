using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using AzR.Core.HelperModels;
using PagedList;
using PagedList.Mvc;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace AzR.WebFw.Heplers
{
    public static class HtmlHelperExtensions
    {
        /*
         * @Html.RadioButtonLabelFor(m => m.IsMarried, true, "Yes, I am married")         
         */
        public static MvcHtmlString RadioButtonLabelFor<TModel, TProperty>(this HtmlHelper<TModel> self, Expression<Func<TModel, TProperty>> expression, bool value, string labelText)
        {
            // Retrieve the qualified model identifier
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = self.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            // Generate the base ID
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.GenerateId(fullName);
            string idAttr = tagBuilder.Attributes["id"];

            // Create an ID specific to the boolean direction
            idAttr = string.Format("{0}_{1}", idAttr, value);

            // Create the individual HTML elements, using the generated ID
            MvcHtmlString radioButton = self.RadioButtonFor(expression, value, new { id = idAttr });
            MvcHtmlString label = self.Label(idAttr, labelText);

            return new MvcHtmlString(radioButton.ToHtmlString() + label.ToHtmlString());
        }
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
       this HtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TProperty>> expression,
       IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list 
                foreach (SelectListItem item in listOfValues)
                {
                    // Generate an id to be given to the radio button field 
                    var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

                    // Create and populate a radio button using the existing html helpers 
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    if (item.Selected)
                    {
                        radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id, @checked = "checked", }).ToHtmlString();
                    }


                    // Create the html string that will be returned to the client 
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
                    sb.AppendFormat("<div class=\"RadioButton\">{0}{1}</div>", radio, label);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString StringRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> list)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model ?? "";
            var sb = new StringBuilder();

            if (list == null) return MvcHtmlString.Create(sb.ToString());
            foreach (var item in list)
            {
                var id = string.Format("{0}_{1}", metaData.PropertyName, item);

                var radio = item != (string)value
                    ? htmlHelper.RadioButtonFor(expression, item, new { id }).ToHtmlString()
                    : htmlHelper.RadioButtonFor(expression, item, new { id, @checked = "checked" }).ToHtmlString();

                sb.AppendFormat("<label class='control-label' id='lable_{2}'>{0}&nbsp;<span class='radio-text'>{1}</span></label>&nbsp;", radio, item, id);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString BooleanRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Dictionary<string, bool> dictionary)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model ?? false;
            var sb = new StringBuilder();

            if (dictionary == null) return MvcHtmlString.Create(sb.ToString());
            foreach (var item in dictionary)
            {
                var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

                var radio = item.Value != (bool)value
                    ? htmlHelper.RadioButtonFor(expression, item.Value, new { id }).ToHtmlString()
                    : htmlHelper.RadioButtonFor(expression, item.Value, new { id, @checked = "checked" }).ToHtmlString();

                sb.AppendFormat("<label class='control-label' id='lable_{2}'>{0}&nbsp;<span class='radio-text'>{1}</span></label>&nbsp;", radio, item.Key, id);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<DropDownItem> options, object htmlAttributes)
        {
            return ComboBoxFor(htmlHelper, expression, options, "--Select--", htmlAttributes);
        }

        public static MvcHtmlString ComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<DropDownItem> options, string optionLable, object htmlAttributes)
        {
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            options = options ?? new List<DropDownItem>();
            var dropdownOptions = value == null
                ? new SelectList(options, "Value", "Text")
                : new SelectList(options, "Value", "Text", value);
            return string.IsNullOrEmpty(optionLable)
                ? htmlHelper.DropDownListFor(expression, dropdownOptions, htmlAttributes)
                : htmlHelper.DropDownListFor(expression, dropdownOptions, optionLable, htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var sb = new StringBuilder();
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var dtpId = "dtp" + metaData.PropertyName;
            var dtp = htmlHelper.TextBoxFor(expression, htmlAttributes).ToHtmlString();
            sb.AppendFormat("<div class='input-group date' id='{0}'> {1} <span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span></span></div>", dtpId, dtp);


            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString TimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var sb = new StringBuilder();
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var dtpId = "tp" + metaData.PropertyName;
            var dtp = htmlHelper.TextBoxFor(expression, htmlAttributes).ToHtmlString();
            sb.AppendFormat("<div class='input-group date' id='{0}'> {1} <span class='input-group-addon'><span class='glyphicon glyphicon-time'></span></span></div>", dtpId, dtp);


            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString DateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string dtpId, string format, object htmlAttributes)
        {
            var sb = new StringBuilder();
            var dtp = htmlHelper.TextBoxFor(expression, format, htmlAttributes).ToHtmlString();
            sb.AppendFormat("<div class='input-group date' id='{0}'> {1} < span class='input-group-addon'><span class='glyphicon glyphicon-calendar'></span></span></div>", dtpId, dtp);


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ImageControl(this HtmlHelper htmlHelper, string fieldName, string imageUrl, object htmlAttributes = null)
        {
            imageUrl = imageUrl ?? "";
            var sb = new StringBuilder();
            var dtp = htmlAttributes == null ? htmlHelper.TextBox(fieldName, "").ToHtmlString() : htmlHelper.TextBox(fieldName, "", htmlAttributes).ToHtmlString();
            dtp = dtp.Replace("type=\"text\" value=\"\"", "type='file'");
            sb.AppendFormat("<span class='btn btn-default btn-file' id='{2}_control'>" +
                            "<img id='{2}_url' src='{0}' class='img-circle' width='50' height='50'>" +
                            "<span id='{2}_name'> Browse For New Image</span>" +
                            "{1}</span>", imageUrl, dtp, fieldName);
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString FileControl(this HtmlHelper htmlHelper, string fieldName, string fileUrl, object htmlAttributes = null)
        {
            fileUrl = fileUrl ?? "";
            var sb = new StringBuilder();
            var dtp = htmlAttributes == null ? htmlHelper.TextBox(fieldName, "").ToHtmlString() : htmlHelper.TextBox(fieldName, "", htmlAttributes).ToHtmlString();
            dtp = dtp.Replace("type=\"text\" value=\"\"", "type='file'");
            sb.AppendFormat("<span class='btn btn-default btn-file' id='{2}_control'>" +
                            "<span id='{2}_name'> Browse For New Image</span>" +
                            "{1}</span>", fileUrl, dtp, fieldName);
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString EnumComboBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<TEnum, bool> predicate, object htmlAttributes) where TEnum : struct, IConvertible
        {
            return EnumComboBoxListFor<TModel, TEnum>(htmlHelper, expression, predicate, null, htmlAttributes);
        }


        /// <summary>
        /// Returns an HTML select element for each value in the enumeration that is
        /// represented by the specified expression and predicate.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TEnum">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display.</param>
        /// <param name="optionLabel">The text for a default empty item. This parameter can be null.</param>
        /// <param name="predicate">A <see cref="Func{TEnum, bool}"/> to filter the items in the enums.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An HTML select element for each value in the enumeration that is represented by the expression and the predicate.</returns>
        /// <exception cref="ArgumentNullException">If expression is null.</exception>
        /// <exception cref="ArgumentException">If TEnum is not Enum Type.</exception>
        public static MvcHtmlString EnumComboBoxListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<TEnum, bool> predicate, string optionLabel, object htmlAttributes) where TEnum : struct, IConvertible
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IList<SelectListItem> selectList = Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Where(e => predicate(e))
                    .Select(e => new SelectListItem
                    {
                        Value = Convert.ToUInt64(e).ToString(),
                        Text = ((Enum)(object)e).GetDisplayName(),
                    }).ToList();
            if (!string.IsNullOrEmpty(optionLabel))
            {
                selectList.Insert(0, new SelectListItem
                {
                    Text = optionLabel,
                });
            }

            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }

        /// <summary>
        /// Gets the name in <see cref="DisplayAttribute"/> of the Enum.
        /// </summary>
        /// <param name="enumeration">A <see cref="Enum"/> that the method is extended to.</param>
        /// <returns>A name string in the <see cref="DisplayAttribute"/> of the Enum.</returns>
        public static string GetDisplayName(this Enum enumeration)
        {
            Type enumType = enumeration.GetType();
            string enumName = Enum.GetName(enumType, enumeration);
            string displayName = enumName;
            try
            {
                MemberInfo member = enumType.GetMember(enumName)[0];

                object[] attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                DisplayAttribute attribute = (DisplayAttribute)attributes[0];
                displayName = attribute.Name;

                if (attribute.ResourceType != null)
                {
                    displayName = attribute.GetName();
                }
            }
            catch { }
            return displayName;
        }

        public static MvcHtmlString PaginationFor(this HtmlHelper html, IPagedList list,
            Func<int, string> generatePageUrl, string updateTargetId = "mainContent")
        {

            var listItems = new List<SelectListItem>
            {
                new SelectListItem {Text = "30", Value = "30"},
                new SelectListItem {Text = "50", Value = "50"},
                new SelectListItem {Text = "100", Value = "100"},
                new SelectListItem {Text = "200", Value = "200"}
            };


            var isDisplay = new PagedListRenderOptions { Display = PagedListDisplayMode.IfNeeded };
            var ajaxOption = new AjaxOptions { HttpMethod = "GET", UpdateTargetId = updateTargetId };
            var pageOption = PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(isDisplay, ajaxOption);
            var pgn = html.PagedListPager(list, generatePageUrl, pageOption);

            var url = "loadPagination('" + generatePageUrl.Invoke(1) + "','" + updateTargetId + "'," +
                      "this" + ")";
            var drop = html.DropDownList("PageSize", new SelectList(listItems, "Value", "Text"),
                new { @class = "form-control pagination-dropdown ", onchange = "url()" });

            var result = list.HasNextPage
                ? string.Format("<div class='row'>" +
                                "<div class='col-md-4'>" +
                                "<div class='form-group' style='width: 200px;'>" +
                                "<label for='PageSize'>Page Size : </label>{0}" +
                                "</div>" +
                                "</div>" +
                                "<div class='col-md-8'>{1}</div>" +
                                "</div>", drop, pgn)
                : string.Format("<div class='row'><div class='col-md-12'>{0}</div></div>", pgn);

            result = result.Replace("url()", url);

            return MvcHtmlString.Create(result);
        }

    }
}