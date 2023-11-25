<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" omit-xml-declaration="no" indent="yes" encoding="US-SCII"/>
	<xsl:strip-space elements="*"/>
		<xsl:template match="/">
			<Products>
			<xsl:for-each select="Root/Products/Product">
				<Product>
					<Name><xsl:value-of select="ProductProfile/Name"/></Name>
					<Code><xsl:value-of select="ProductProfile/Id"/></Code>
					<Category><xsl:value-of select="ProductProfile/Category"/></Category>
					<Collection><xsl:value-of select="ProductProfile/Collection"/></Collection>
					<Tags>
						<xsl:for-each select="ProductProfile/Tags/Tag">
							<Tag><xsl:value-of select="." /></Tag>
						</xsl:for-each>
					</Tags>
					<Size><xsl:value-of select="Specification/Height"/>x<xsl:value-of select="Specification/Width"/><xsl:value-of select="Specification/Unit"/></Size>
					<MRP><xsl:value-of select="Cost/MRP"/></MRP>
					<DiscountPrice><xsl:value-of select="Cost/DiscountPrice"/></DiscountPrice>
				</Product>
			</xsl:for-each>
			
			</Products>
		</xsl:template>
	
</xsl:stylesheet>
