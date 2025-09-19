### Editable Table
When making a table eitable, there needs to be support for validation. Validation should stylize the content in a way that it shows red for error, as well as have an error message as to why it failed.

When it comes to validation, it should occur when the user tries to commit the change (input loses focus or the user presses enter). It should also do a "pre validation" if the user has stopped typing for x time. Not sure what the time should be, but something other than "every keystroke". The only time every keystroke should be allowed is if we are executing clientside validation.

