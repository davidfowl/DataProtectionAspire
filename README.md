
Instructions:

1. Get a daily build of AZD:
https://github.com/Azure/azure-dev/tree/main/cli/installer#download-from-daily-builds


2. Set the feature flag
azd config set alpha.aspire.autoConfigureDataProtection on

How to use the app:
 
- `/protect` - Returns a data protection payload and the pod name
- `/unprotect/{secret}` - Will return the unprotected payload and pod name
 
