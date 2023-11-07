<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	
		<xsl:template match="/">
			<Products>
			<xsl:for-each select="Root/Products/Product">
				<Name>
					<xsl:value-of select="ProductProfile/Name"/>
				</Name>
			</xsl:for-each>
			
			</Products>
		</xsl:template>
	
</xsl:stylesheet>
