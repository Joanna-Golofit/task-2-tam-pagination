<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" encoding="UTF-8" />
    <xsl:template match="/sites">
        <html lang="en">
            <head>
                <title>Sites</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
                <link rel="stylesheet" href="https://cdn.jsdelivr.net/gh/kognise/water.css@latest/dist/dark.css"></link>
                <style>
                    table {
                        width: 100%;
                    }
                </style>
            </head>
            <body>
                <h1>Sites</h1>
                <table>
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Updated</th>
                            <th>App</th>
                            <th>Identity</th>
                            <th>Api</th>
                        </tr>
                    </thead>
                    <tbody>
                        <xsl:for-each select="./site">
                            <tr>
                                <td>
                                    <xsl:value-of select="@name" />
                                </td>
                                <td>
                                    <xsl:value-of select="@date" />
                                </td>
                                <td>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@name" />/app
                                        </xsl:attribute>
                                        open
                                    </a>
                                </td>
                                <td>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@name" />/identity
                                        </xsl:attribute>
                                        open
                                    </a>
                                </td>
                                <td>
                                    <a>
                                        <xsl:attribute name="href">
                                            <xsl:value-of select="@name" />/api/api/version
                                        </xsl:attribute>
                                        open
                                    </a>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </tbody>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
