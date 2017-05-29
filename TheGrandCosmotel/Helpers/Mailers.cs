using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Helpers
{
    public class Mailers
    {
        public static string Register()
        {
            return @"


<!DOCTYPE html>
<html>

<head>
    <title>Welcome</title>
    <meta charset=""utf-8"" />
    <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,700&amp;subset=greek"" rel=""stylesheet"">
</head>

<body style=""background:#000000; font-family: Montserrat, helvetica, san-serif; margin: 0px; padding: 0px; width: 100%; height: 100%;"">
    <table cellpadding=""0"" cellspacing=""0"" width=""100%"" height=""100%"">
        <tbody style=""padding: 0px; margin: 0px; line-height: 100%"">
            <tr>
                <td align=""center"" valign=""top"">
                    <table style=""width: 600px;"">
                        <tr>
                            <td>
                                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""height: 182px;"">
                                    <tbody style=""padding: 0px; margin: 0px; line-height: 100%"">
                                        <tr>
                                            <td>
                                            <a style=""text-decoration: none;"" href=""{0}"">
                                                <img src=""https://www.amusegr.eu/welcome.jpg"" alt="""">
                                            </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</body>

</html>
            ";
        }
            

        public static string Recover()
        {
            return
            @"

<!DOCTYPE html>
<html>

<head>
    <title>Welcome</title>
    <meta charset=""utf-8"" />
    <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,700&amp;subset=greek"" rel=""stylesheet"">
</head>

<body style=""background:#000000; font-family: Montserrat, helvetica, san-serif; margin: 0px; padding: 0px; width: 100%; height: 100%;"">
    <table cellpadding=""0"" cellspacing=""0"" width=""100%"" height=""100%"">
        <tbody style=""padding: 0px; margin: 0px; line-height: 100%"">
            <tr>
                <td align=""center"" valign=""top"">
                    <table style=""width: 600px;"">
                        <tr>
                            <td>
                                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""height: 182px;"">
                                    <tbody style=""padding: 0px; margin: 0px; line-height: 100%"">
                                        <tr>
                                            <td>
                                                <a style=""text-decoration: none;"" href=""{0}"">
                                                    <img src=""https://www.amusegr.eu/top.jpg"" alt="""">
                                                </a>
                                            </td>
                                        </tr>
                                        <tr style=""background: #000000"">
                                            <td align=""center"">
                                                <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                    <tbody style=""padding: 0px; margin: 0px; line-height: 100%"">
                                                        <tr>
                                                            <td>
                                                                <p align=""left"" style=""font-family: 'Open Sans', Verdana, Helvetica, sans-serif; font-size: 16px; font-weight: bold; color:#ffffff; line-height:17px; margin: 0px 35px 0px 40px;"">
                                                                    Αγαπητέ/ή {1},
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <a style=""text-decoration: none;"" href=""{0}"">
                                                    <img src=""https://www.amusegr.eu/bottom.jpg"" alt=""""></a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</body>

</html>

                ";
        }

    }
}