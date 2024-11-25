using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using IC_Eval_FE.Services.Helper_Functions;

namespace IC_Eval_FE.FragmentFactory
{
    public class FieldRenderer
    {
        public Validation validation = new Validation();
        public void RenderTextField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)

        {
            builder.AddAttribute(4, "Value", userBindings.UserText);
            builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserText = value));
            formDataToJson[field.Label] = userBindings?.UserText ?? "";
        }

        public void RenderEmailField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            bool isEmailValid = true;
            bool isRequired = field.Required;

            if (isRequired && !string.IsNullOrEmpty(userBindings.UserEmail))
            {
                isEmailValid = validation.ValidateEmail(userBindings.UserEmail); 
            }


            builder.AddAttribute(1, "Value", userBindings.UserEmail); 
            builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, value =>
            {
                userBindings.UserEmail = value; 
            }));

            
            builder.AddAttribute(3, "Error", !isEmailValid && isRequired);  
            builder.AddAttribute(4, "ErrorText", !isEmailValid && isRequired ? "Invalid email format" : null);  

            
            formDataToJson[field.Label] = userBindings?.UserEmail ?? "";
        }

        public void RenderNumberField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(8, "Min", field.Min);
            builder.AddAttribute(9, "Max", field.Max);
            builder.AddAttribute(10, "Value", userBindings.UserNum);
            builder.AddAttribute(11, "ValueChanged", EventCallback.Factory.Create<int>(this, value => userBindings.UserNum = value));
            formDataToJson[field.Label] = userBindings.UserNum;
        }

        public void RenderDropdownField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(12, "Value", userBindings.UserDropDown);
            builder.AddAttribute(13, "ValueChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserDropDown = value));
            formDataToJson[field.Label] = userBindings?.UserDropDown ?? "";
            builder.AddAttribute(14, "ChildContent", (RenderFragment)(dropdownBuilder =>
            {
                foreach (var state in field.Values)
                {
                    dropdownBuilder.OpenComponent<MudSelectItem<string>>(0);
                    dropdownBuilder.AddAttribute(15, "Value", state);
                    dropdownBuilder.AddAttribute(16, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.AddContent(0, state);
                    }));
                    dropdownBuilder.CloseComponent();
                }
            }));
        }

        public void RenderCheckboxField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(17, "Checked", userBindings.UserRequired);
            builder.AddAttribute(18, "ValueChanged", EventCallback.Factory.Create<bool>(this, value => userBindings.UserRequired = value));
            formDataToJson[field.Label] = userBindings.UserRequired;
        }

        public void RenderDateField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(19, "Value", userBindings.UserDate);
            builder.AddAttribute(20, "DateChanged", EventCallback.Factory.Create<DateTime?>(this, value => userBindings.UserDate = value));
            formDataToJson[field.Label] = userBindings?.UserDate ?? DateTime.MinValue;
        }

        public void RenderDateTimeField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(21, "Value", userBindings.UserDateTime);
            builder.AddAttribute(22, "DateChanged", EventCallback.Factory.Create<DateTime?>(this, value => userBindings.UserDateTime = value));
            formDataToJson[field.Label] = userBindings?.UserDate ?? DateTime.MinValue;
        }

        public void RenderTimeField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(23, "Time", userBindings.UserTime);
            builder.AddAttribute(24, "AmPm", true);
            builder.AddAttribute(25, "Direction", Direction.Top);
            builder.AddAttribute(26, "TimeChanged", EventCallback.Factory.Create<TimeSpan?>(this, value => userBindings.UserTime = value));
            formDataToJson[field.Label] = userBindings?.UserTime ?? TimeSpan.Zero;
        }

        public void RenderRangeField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(27, "Value", userBindings.UserRange);
            builder.AddAttribute(28, "Min", field.Min);
            builder.AddAttribute(29, "Max", field.Max);
            builder.AddAttribute(30, "ValueChanged", EventCallback.Factory.Create<int>(this, value => userBindings.UserRange = value));
            builder.AddAttribute(31, "Color", Color.Info);
            builder.AddAttribute(32, "ChildContent", (RenderFragment)((builder2) =>
            {
                builder2.AddContent(0, $"{field.Label}: {userBindings.UserRange}");
            }));
            formDataToJson[field.Label] = userBindings?.UserRange ?? 0;
        }

        public void RenderRadioField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(33, "Value", userBindings.UserRadio);
            builder.AddAttribute(34, "ValueChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserRadio = value));
            builder.AddAttribute(35, "ChildContent", (RenderFragment)(radioBuilder =>
            {
                foreach (var option in field.Values)
                {
                    radioBuilder.OpenComponent<MudRadio<string>>(0);
                    radioBuilder.AddAttribute(36, "Value", option);
                    radioBuilder.AddAttribute(37, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.AddContent(0, option);
                    }));
                    radioBuilder.CloseComponent();
                }
            }));
            formDataToJson[field.Label] = userBindings?.UserRadio ?? "";
        }

        public void RenderColorField(RenderTreeBuilder builder, FormField field, UserBindings userBindings, Dictionary<string, object> formDataToJson)
        {
            builder.AddAttribute(38, "Text", userBindings.UserColor);
            //builder.AddAttribute(39, "TextChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserColor = value));
            formDataToJson[field.Label] = userBindings?.UserColor ?? "";
        }
    }
}
