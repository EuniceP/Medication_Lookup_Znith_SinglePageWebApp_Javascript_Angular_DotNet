<%@ Page Language="C#" %>

<!DOCTYPE html>

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="x-ua-compatible" content="IE=9" />
     <script type="text/javascript" src="js/vendors/jquery/jquery-2.2.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            runChrome("http://whlweb25/pharma_lookup");
        });
        function CloseWindow() {
            window.open("", "_self", "");
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script language="vbscript" type="text/vbscript">
            Option Explicit 
            On Error Resume Next   
            Function runChrome(url) 
                dim objShell
				set objShell = CreateObject("Shell.Application")
                objShell.ShellExecute "chrome.exe", url, "", "", 1
                set objShell = Nothing
                If Err.number = 0 Then
                    CloseWindow()
                End If
            End Function                  
        </script>
        <div>
            ...Redirecting to Chrome Browser...
        </div>
    </form>
</body>
</html>
