<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- Корень -->
  <xsl:template match="/Pay">
    <Employees>
      <!-- Все item -->
      <xsl:variable name="allItems" select="january/item | february/item | march/item"/>

      <!-- Группировка по сотруднику -->
      <xsl:for-each select="$allItems">
        <xsl:variable name="empKey" select="concat(@name, '|', @surname)"/>

        <!-- Только ПЕРВОЕ вхождение сотрудника -->
        <xsl:if test="generate-id() = generate-id($allItems[concat(@name, '|', @surname) = $empKey][1])">
          
          <Employee name="{@name}" surname="{@surname}">
            <!-- Все зарплаты этого сотрудника, отсортированные по месяцам -->
            <xsl:for-each select="$allItems[concat(@name, '|', @surname) = $empKey]">
              
              <!-- Сортировка: january → february → march -->
              <xsl:sort select="count(ancestor::*[name()='january']) * 1 +
                                count(ancestor::*[name()='february']) * 2 +
                                count(ancestor::*[name()='march']) * 3" 
                        data-type="number"/>

              <!-- Месяц из родителя -->
              <xsl:variable name="month">
                <xsl:choose>
                  <xsl:when test="ancestor::january">january</xsl:when>
                  <xsl:when test="ancestor::february">february</xsl:when>
                  <xsl:when test="ancestor::march">march</xsl:when>
                </xsl:choose>
              </xsl:variable>

              <salary amount="{@amount}" mount="{$month}"/>
            </xsl:for-each>
          </Employee>
        </xsl:if>
      </xsl:for-each>
    </Employees>
  </xsl:template>

</xsl:stylesheet>