using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;
using IC_Eval_FE.Services.Helper_Functions;
using IC_Eval_FE.Components;


namespace IC_Eval_FE.Pages
{
    public partial class Home : ComponentBase
    {

        // Fields for form state and control
        protected bool success;
        private bool isRequired;
        private string formTitle = string.Empty;
        private string jsonResponse;
        private bool isErrorFetchingConfig = false;
        private string errorMessage = string.Empty;

        // Form controls and bindings
        MudTextField<string> pwField1;
        MudForm form;
        UserBindings userBindings = new();

        // Validation instance
        Validation validation = new Validation();

        // Form fields and data
        private List<FormField> formFields = new List<FormField>();
        private Dictionary<string, object?> formDataToJson = new Dictionary<string, object?>();

        

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetFromJsonAsync<FormModel>(jsonResponse);

            try
            {
                if (response != null)
                {
                    formTitle = response.Title;
                    formFields = response.Fields;

                    formDataToJson = formFields.ToDictionary(
                        field => field.Label,
                        field => field.Type switch
                        {
                            "text" => (object)string.Empty,       
                            "number" => (object)0,               
                            "checkbox" => (object)false,         
                            "dropdown" => null,         
                            _ => null
                        }
                        );


                    var emailField = response.Fields.FirstOrDefault(f => f.Type == AppStrings.Email);

                    if (emailField != null)
                    {

                        isRequired = emailField.Required;

                    }
                    else
                    {

                        isRequired = false;
                    }

                    isErrorFetchingConfig = false;
                    errorMessage = string.Empty;
                }
            }
            catch (HttpRequestException httpEx)
            {

                Console.WriteLine($"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                isErrorFetchingConfig = true;  
                errorMessage = $"{AppStrings.ErrFetchConfig} {ex.Message}";  
                Console.WriteLine($"{AppStrings.ErrFetchConfig} {ex.Message}");
            }


        }

        private Type GetComponentType(string type)
        {
            return type switch
            {
                AppStrings.text => typeof(TextComponent),
                AppStrings.email => typeof(EmailComponent),
                AppStrings.number => typeof(NumberComponent),
                AppStrings.dropdown => typeof(DropDownComponent),
                AppStrings.checkbox => typeof(CheckboxComponent),
                AppStrings.date => typeof(DateComponent),
                AppStrings.datetime => typeof(DatetimeComponent),
                AppStrings.time => typeof(TimeComponent),
                AppStrings.range => typeof(RangeComponent),
                AppStrings.radio => typeof(RadioComponent),
                AppStrings.color => typeof(ColorComponent),
                AppStrings.textarea => typeof(TextAreaComponent),
                _ => throw new ArgumentException($"Unsupported field type: {type}")
            };
        }

        private RenderFragment RenderField(FormField field) => builder =>
        {
            builder.OpenRegion(0); 
            builder.OpenComponent(0, GetComponentType(field.Type)); 

            builder.AddAttribute(1, "Label", field.Label);
            builder.AddAttribute(2, "Required", field.Required);
            builder.AddAttribute(3, "formDataToJson", formDataToJson);

            if (field.Type == AppStrings.number || field.Type == AppStrings.range)
            {
                builder.AddAttribute(4, "Min", field.Min);
                builder.AddAttribute(5, "Max", field.Max);
            }

            if (field.Type == AppStrings.dropdown || field.Type == AppStrings.radio)
            {
                builder.AddAttribute(6, "Values", field.Values);
            }

            builder.CloseComponent(); 
            builder.CloseRegion();
        };

        private void HandleValidSubmit()
        {
            form.Validate();

            bool isEmailValid = false;

            if (formDataToJson["Email"] != null)
            {
                isEmailValid = validation.ValidateEmail(formDataToJson["Email"].ToString());
            }


            if ((!isEmailValid && isRequired) || !form.IsValid)
            {

                jsonResponse = AppStrings.InvalidFormInput;
                return;
            }

            
            if (form.IsValid)
            {

                jsonResponse = JsonSerializer.Serialize(formDataToJson);

            }

        }

        public async Task FetchFormConfig()
        {
            try
            {

                var response = await Http.GetFromJsonAsync<FormModel>(AppStrings.data);

                if (response != null)
                {

                    formTitle = response.Title;
                    formFields = response.Fields;


                    StateHasChanged();
                }
            }
            catch (HttpRequestException httpEx)
            {
                
                Console.WriteLine($"{AppStrings.RequestError} {httpEx.Message}");
            }
            catch (Exception ex)
            {
                isErrorFetchingConfig = true;  
                errorMessage = $"{AppStrings.ErrFetchServer} {ex.Message}";  
                Console.WriteLine($"{AppStrings.ErrFetchConfig} {ex.Message}");
            }
        }


    }
}