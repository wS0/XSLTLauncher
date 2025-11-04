<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- Корневой шаблон -->
  <xsl:template match="/Pay">
    <Employees>
      <!-- Группируем по сотруднику (name + surname) -->
      <xsl:for-each select="item">
        <xsl:sort select="concat(@name, ' ', @surname)" />
        
        <!-- Ключ для группировки -->
        <xsl:variable name="empKey" select="concat(@name, '|', @surname)" />
        
        <!-- Проверяем, не обработали ли уже этого сотрудника -->
        <xsl:if test="not(preceding-sibling::item[
                         concat(@name, '|', @surname) = $empKey])">
          
          <Employee name="{@name}" surname="{@surname}">
            <!-- Все зарплаты этого сотрудника -->
            <xsl:for-each select="../item[
                                  concat(@name, '|', @surname) = $empKey]">
              <xsl:sort select="@mount" />
              
              <salary amount="{@amount}" mount="{@mount}" />
            </xsl:for-each>
          </Employee>
        </xsl:if>
      </xsl:for-each>
    </Employees>
  </xsl:template>

</xsl:stylesheet>