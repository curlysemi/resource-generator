# resource-generator
Creates string resources for a UWP, WPF or Web Application.

## C# Templates
Support for C# and JS templating. Use handlebars-style template strings and functions with parameters from the template will be generated instead of properties.

### Example

| Name | Value | Comment |
|------|-------|---------|
|`Example`|`This is a {{adjective}} example.`|    |
-------------------------

### Output
```csharp
        /// <summary>
        /// Looks up a localized string similar to "This is a {{adjective}} example.".
        /// </summary>
        public static string Example(object adjective)
        {
            return ExampleFileName.ResourceManager.GetString("Example", ExampleFileName.CultureInfo).Replace("{{adjective}}", adjective.ToString());
        }
```



## JS Templates
For JS templates, first define a JS template by creating an entry starting with "`__TEMPLATE_NAME__`". Any characters after this prefix will be the ID of the JS template. Add a `*` at the end of a text template name and an 'overload' will be generated that consumes the JS template.

The constants that you can use to make JS templates that can fit your pipeline are:
* `__TARGET_CLASS_NAME__` — which is the filename of the `.resx` file that the `ResourceGenerator` custom tool is being applied to.
* `__TEMPLATE_NAME__` — which is the name of the text template string.
* `__NAMED_PARAMS_OBJ__` — which is a JS object containing all of the parameters to pass to your JS templating solution.

### Example
| Name | Value | Comment |
|------|-------|---------|
|`__TEMPLATE_NAME__ko`|`$root.template(window.Resources.__TARGET_CLASS_NAME__.__TEMPLATE_NAME__, __NAMED_PARAMS_OBJ__)`|   |
|`Example*ko`|`This is a {{adjective}} example.`|    |
-------------------------

### Output
```csharp
        /// <summary>
        /// Looks up a localized string similar to "This is a {{adjective}} example.".
        /// </summary>
        public static string Example(object adjective)
        {
            return ExampleFileName.ResourceManager.GetString("Example", ExampleFileName.CultureInfo).Replace("{{adjective}}", adjective.ToString());
        }

        /// <summary>
        /// Looks up a localized string similar to "This is a {{adjective}} example.".
        /// </summary>
        public static string ko__Example(object adjective)
        {
            return ExampleFileName.ResourceManager.GetString("__RESOURCE_TEMPLATE__ko").Replace("__TEMPLATE_NAME__", "Example").Replace("__TARGET_CLASS_NAME__", "ExampleFileName").Replace("__NAMED_PARAMS_OBJ__", "{ adjective: '__adjective__' }").Replace("__adjective__", adjective.ToString());
        }
```

