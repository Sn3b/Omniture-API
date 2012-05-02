If you wish to update the reference to the WSDL, follow these simple steps:
1. Open the .wsdl file with your favourite text editor, search for the exact string "tns:string", replace by "xsd:string" (1 occurence)
2. Refresh/Update/Remove+Add the WSDL as service reference (I like to call it Api{version} eg. "Api13")
3. Open the Reference.cs file created in {SolutionFolder}\Omniture\Service References\Api13\ and remove the dots (".") from all the namespace.method names:
- CodeManager
- Company
- Dashboard
- DataSource
- DataWarehouse
- DeliveryList
- Discover
- Logs
- Permissions
- Report
- ReportSuite
- Saint
- Scheduling
- Survey
- User

For each namespace above do search & replace on "namespace. replace by "namespace
Example:
- Search for "CodeManager.
- Replace by "CodeManager