# IC-Eval-FE

# Development Tasks (Cross reference with appropriate branch):

## Frontend (FE)

- **FE-1**: Clear default pages in Blazor to prep for form development.
- **FE-2**: Install MudBlazor packages and other required packages.
- **FE-3**: Import default MudBlazor form as a template.
- **FE-4**: Create models based on json structure.
- **FE-5**: Try API call from backend.
- **FE-6**: Figure out how to dynamically create form fields as components.
- **FE-7**: Figure out how to convert user inputs into json response.
- **FE-8**: Refactor code behind and abstract away functions if neccesary.
- **FE-9**: Abstract away fragment creation into separate functions.
- **FE-10**: Email formatting.
- **FE-11**: Code cleanup.


## Assumptions:
- The JSON file is always a valid form configuration.
- No file uploads are allowed since it's difficult to represent in a user response JSON.

## Steps to Run the Application:

1. **Run the Backend:**
   - Follow the steps in the [IC-Eval-Backend GitHub repository](https://github.com/echen12/IC-Eval-Backend) to run the backend first.

2. **Clone the Application:**
   - Clone the Blazor WebAssembly (WASM) application to your local machine.

3. **Build the Application:**
   - Open a developer PowerShell or terminal window.
   - Navigate to the application directory.
   - Run the following command to build the application:
     ```bash
     dotnet build
     ```

4. **Run the Application:**
   - Start the application with live reload using `dotnet watch`:
     ```bash
     dotnet watch
     ```

---
