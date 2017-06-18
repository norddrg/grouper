<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:key name="exclude-search" match="wix:Component[contains(wix:File/@Source, '.pdb') or contains(wix:File/@Source, '.vshost.exe') ]" use="@Id"/>

  <xsl:template match="wix:Component[key('exclude-search', @Id)]"/>
  <xsl:template match="wix:ComponentRef[key('exclude-search', @Id)]"/>

</xsl:stylesheet>