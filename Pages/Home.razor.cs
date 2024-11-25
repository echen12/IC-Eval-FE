using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;
using IC_Eval_FE.FragmentFactory;
using IC_Eval_FE.Services.Helper_Functions;
using Microsoft.VisualBasic.FileIO;


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
        private Dictionary<string, object> formDataToJson = new Dictionary<string, object>();

        // FieldRenderer instance for dynamic field rendering
        private FieldRenderer fieldRenderer = new FieldRenderer();

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetFromJsonAsync<FormModel>(jsonResponse);

            try
            {
                if (response != null)
                {
                    formTitle = response.Title;
                    formFields = response.Fields;


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

        // Determine type of Mud fragment required
        private Type GetFieldComponentType(string type)
        {
            return type switch
            {
                AppStrings.text => typeof(MudTextField<string>),
                AppStrings.email => typeof(MudTextField<string>),
                AppStrings.number => typeof(MudNumericField<int>),
                AppStrings.dropdown => typeof(MudSelect<string>),
                AppStrings.checkbox => typeof(MudCheckBox<bool>),
                AppStrings.date => typeof(MudDatePicker),
                AppStrings.datetime => typeof(MudDatePicker),
                AppStrings.time => typeof(MudTimePicker),
                AppStrings.textarea => typeof(MudTextField<string>),
                AppStrings.range => typeof(MudSlider<int>),
                AppStrings.color => typeof(MudColorPicker),
                AppStrings.radio => typeof(MudRadioGroup<string>),

                _ => typeof(MudTextField<string>) // Default case
            };
        }

        // Dynamically add attributes
        private RenderFragment RenderField(FormField field) => builder =>
        {

            builder.OpenComponent(0, GetFieldComponentType(field.Type)); 
            builder.AddAttribute(1, AppStrings.Label, field.Label); 
            builder.AddAttribute(2, AppStrings.Required, field.Required); 
            builder.AddAttribute(3, AppStrings.Class, "mud-width-full");


            switch (field.Type)
            {
                case AppStrings.text:
                    fieldRenderer.RenderTextField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.email:
                    fieldRenderer.RenderEmailField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.number:
                    fieldRenderer.RenderNumberField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.dropdown:
                    fieldRenderer.RenderDropdownField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.checkbox:
                    fieldRenderer.RenderCheckboxField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.date:
                    fieldRenderer.RenderDateField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.datetime:
                    fieldRenderer.RenderDateTimeField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.time:
                    fieldRenderer.RenderTimeField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.range:
                    fieldRenderer.RenderRangeField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.radio:
                    fieldRenderer.RenderRadioField(builder, field, userBindings, formDataToJson);
                    break;

                case AppStrings.color:
                    fieldRenderer.RenderColorField(builder, field, userBindings, formDataToJson);
                    break;

                default:
                    break;
            }


            builder.CloseComponent();
        };

        private void HandleValidSubmit()
        {
            form.Validate();
            bool isEmailValid = validation.ValidateEmail(userBindings.UserEmail);
            

            if (!isEmailValid && isRequired)
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