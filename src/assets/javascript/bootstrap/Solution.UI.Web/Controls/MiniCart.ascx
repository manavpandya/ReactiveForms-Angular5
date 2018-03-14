<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiniCart.ascx.cs" Inherits="Solution.UI.Web.Controls.MiniCart" %>
<input type="hidden" id="hiddenCustID" value='<%=CustomerID %>' />
<input type="hidden" id="hiddenTotalItems" value='<%=TotalItems %>' />
<div id="CartLayer" class="right-sub-content" onmouseover="showMiniCart();" onmouseout="resetHover();">
    <div id="DivMyCart" class="right-sub-row1">
        <div id="divMiniCart">
            <div id="divCart">
                 <asp:Literal ID="litMiniCart" runat="server"></asp:Literal>
               <%-- <table>
                    <tr style="padding-top: 0px; width: 300px;">
                        <td style="padding-top: 5px; width: 290px;" class="shopping_burron">
                            <p>
                                Your Shopping Cart</p>
                        </td>
                    </tr>
                </table>--%>
            </div>
        </div>
    </div>
</div>
