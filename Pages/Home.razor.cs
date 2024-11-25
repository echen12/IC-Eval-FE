﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using System.Net.Http.Json;
using System.Text.Json;


namespace IC_Eval_FE.Pages  
{
    public partial class Home : ComponentBase
    {

        protected bool success;
        private string formTitle = string.Empty;
        string[] errors = { };
        protected string jsonResponse;
        MudTextField<string> pwField1;
        MudForm form;

        UserBindings userBindings = new();

        private List<FormField> formFields = new List<FormField>();

        private Dictionary<string, object> formDataToJson = new Dictionary<string, object>();

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetFromJsonAsync<FormModel>(jsonResponse);

            try
            {
                if (response != null)
                {
                    formTitle = response.Title;
                    formFields = response.Fields;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching from server: {ex.Message}");
            }


        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }

        // Determine type of Mud fragment required
        private Type GetFieldComponentType(string type)
        {
            return type switch
            {
                "text" => typeof(MudTextField<string>),
                "email" => typeof(MudTextField<string>),
                "number" => typeof(MudNumericField<int>),
                "dropdown" => typeof(MudSelect<string>),
                "checkbox" => typeof(MudCheckBox<bool>),
                "date" => typeof(MudDatePicker),
                "datetime" => typeof(MudDatePicker),
                "time" => typeof(MudTimePicker),
                "textarea" => typeof(MudTextField<string>),
                "range" => typeof(MudSlider<int>),
                "color" => typeof(MudColorPicker),
                "radio" => typeof(MudRadioGroup<string>),

                _ => typeof(MudTextField<string>)
            };
        }

        // Dynamically add attributes
        private RenderFragment RenderField(FormField field) => builder =>
        {

            builder.OpenComponent(0, GetFieldComponentType(field.Type));
            builder.AddAttribute(1, "Label", field.Label);
            builder.AddAttribute(2, "Required", field.Required);
            builder.AddAttribute(3, "Class", "mud-width-full");


            switch (field.Type)
            {

                case "text":
                    builder.AddAttribute(4, "Value", userBindings.UserText);
                    builder.AddAttribute(5, "ValueChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserText = value));
                    formDataToJson[field.Label] = userBindings?.UserText ?? "";
                    break;

                case "email":
                    builder.AddAttribute(6, "Value", userBindings.UserEmail);
                    builder.AddAttribute(7, "ValueChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserEmail = value));
                    formDataToJson[field.Label] = userBindings?.UserEmail ?? "";
                    break;

                case "number":
                    builder.AddAttribute(8, "Min", field.Min);
                    builder.AddAttribute(9, "Max", field.Max);
                    builder.AddAttribute(10, "Value", userBindings.UserNum);
                    builder.AddAttribute(11, "ValueChanged", EventCallback.Factory.Create<int>(this, value => userBindings.UserNum = value));
                    formDataToJson[field.Label] = userBindings.UserNum;
                    break;

                case "dropdown":
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
                    break;

                case "checkbox":
                    builder.AddAttribute(17, "Checked", userBindings.UserRequired);
                    builder.AddAttribute(18, "ValueChanged", EventCallback.Factory.Create<bool>(this, value => userBindings.UserRequired = value));
                    formDataToJson[field.Label] = userBindings.UserRequired;
                    break;

                case "date":
                    builder.AddAttribute(19, "Value", userBindings.UserDate);
                    builder.AddAttribute(20, "DateChanged", EventCallback.Factory.Create<DateTime?>(this, value => userBindings.UserDate = value));
                    formDataToJson[field.Label] = userBindings?.UserDate ?? DateTime.MinValue;
                    break;

                case "datetime":
                    builder.AddAttribute(21, "Value", userBindings.UserDateTime);
                    builder.AddAttribute(22, "DateChanged", EventCallback.Factory.Create<DateTime?>(this, value => userBindings.UserDateTime = value));
                    formDataToJson[field.Label] = userBindings?.UserDate ?? DateTime.MinValue;
                    break;

                case "time":
                    builder.AddAttribute(23, "Time", userBindings.UserTime);
                    builder.AddAttribute(24, "AmPm", true);
                    builder.AddAttribute(25, "Direction", Direction.Top);
                    builder.AddAttribute(26, "TimeChanged", EventCallback.Factory.Create<TimeSpan?>(this, value => userBindings.UserTime = value));
                    formDataToJson[field.Label] = userBindings?.UserTime ?? TimeSpan.Zero;
                    break;

                case "range":
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
                    break;

                case "radio":
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
                    break;

                case "color":
                    builder.AddAttribute(38, "Text", userBindings.UserColor);
                    builder.AddAttribute(39, "TextChanged", EventCallback.Factory.Create<string>(this, value => userBindings.UserColor = value));
                    formDataToJson[field.Label] = userBindings?.UserColor ?? "";
                    break;

                default:
                    break;
            }


            builder.CloseComponent();
        };

        private void HandleValidSubmit()
        {
            form.Validate();
            if (form.IsValid)
            {

                jsonResponse = JsonSerializer.Serialize(formDataToJson);

            }

        }

        private void ClearForm()
        {
            form.ResetAsync();
        }

        public async Task FetchFormConfig()
        {
            try
            {

                var response = await Http.GetFromJsonAsync<FormModel>("data");

                if (response != null)
                {

                    formTitle = response.Title;
                    formFields = response.Fields;


                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error fetching form config: {ex.Message}");
            }
        }
    }
}