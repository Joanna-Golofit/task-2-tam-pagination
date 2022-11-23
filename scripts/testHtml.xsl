<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" encoding="UTF-8" />
    <xsl:template match="/testsuites">
        <html lang="en">
            <head>
                <title>Test Reports</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
                <link rel="stylesheet" href="https://unpkg.com/purecss@1.0.0/build/pure-min.css" integrity="sha384-nn4HPE8lTHyVtfCBi5yW9d20FjT8BJwUXyWZT9InLYax14RDjBj46LmSztkmNP9w" crossorigin="anonymous">
                </link>
                <style>
    .home-menu {
        padding: 0.5em;
        text-align: center;
        box-shadow: 0 1px 1px rgba(0,0,0, 0.10);
    }
    .home-menu {
        background: #2d3e50;
    }

    .home-menu .pure-menu-heading {
        color: white;
        font-weight: 400;
        font-size: 120%;
    }

    .home-menu .pure-menu-selected a {
        color: white;
    }

    .home-menu a {
        color: #6FBEF3;
    }
    .home-menu li a:hover,
    .home-menu li a:focus {
        background: none;
        border: none;
        color: #AECFE5;
    }

    .container {
        padding: 20px;
        padding-top: 60px;
    }
    table {
        width: 100%;
    }
    .failed {
        background: #f44336;
    }
    .failed-text {
        background: #ffcdd2;
    }
                </style>
            </head>
            <body>
                <div class="header">
                    <div class="home-menu pure-menu pure-menu-horizontal pure-menu-fixed">
                        <a class="pure-menu-heading" href="">Test reports</a>
                    </div>
                </div>
                <div class="container">
                    <xsl:for-each select="./testsuite">
                        <h2>
                            Tests for: <xsl:value-of select="@name" />
                        </h2>
                        <h3>Summary</h3>
                        <table class="pure-table pure-table-bordered">
                            <thead>
                                <tr>
                                    <th>Time</th>
                                    <th>Tests</th>
                                    <th>Errors</th>
                                    <th>Failures</th>
                                    <th>Skipped</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <xsl:value-of select="sum(./testcase/@time)" />s
                                    </td>
                                    <td>
                                        <xsl:value-of select="@tests" />
                                    </td>
                                    <td>
                                        <xsl:value-of select="@errors" />
                                    </td>
                                    <td>
                                        <xsl:value-of select="@failures" />
                                    </td>
                                    <td>
                                        <xsl:value-of select="@skipped" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <h3>Tests</h3>
                        <table class="pure-table pure-table-bordered">
                            <thead>
                                <tr>
                                    <th>Class</th>
                                    <th>Name</th>
                                    <th>Time</th>
                                </tr>
                            </thead>
                            <tbody>
                                <xsl:for-each select="./testcase">
                                    <tr>
                                        <xsl:attribute name="class">
                                            <xsl:if test="./failure">
                                                failed
                                            </xsl:if>
                                        </xsl:attribute>
                                        <td>
                                            <xsl:value-of select="@classname" />
                                        </td>
                                        <td>
                                            <xsl:value-of select="@name" />
                                        </td>
                                        <td>
                                            <xsl:value-of select="@time" />s
                                        </td>
                                    </tr>
                                    <xsl:if test="./failure">
                                        <tr class="failed-text">
                                            <td colspan="3">
                                                <xsl:value-of select="./failure/text()"/>
                                            </td>
                                        </tr>
                                    </xsl:if>
                                </xsl:for-each>
                            </tbody>
                        </table>
                    </xsl:for-each>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>