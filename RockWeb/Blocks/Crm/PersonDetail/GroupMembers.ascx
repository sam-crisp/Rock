﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupMembers.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.GroupMembers" %>

<asp:UpdatePanel ID="upGroupMembers" runat="server">
    <ContentTemplate>

        <div class="persondetails-grouplist js-grouplist-sort-container">

            <asp:Repeater ID="rptrGroups" runat="server" >
            <ItemTemplate>

                <asp:Panel ID="pnlGroup" runat="server" CssClass="persondetails-group js-persondetails-group panel-widget">
                    <asp:HiddenField ID="hfGroupId" runat="server" Value='<%# Eval("Id") %>' />
                    <header>
                        <a id="lReorderIcon" runat="server" class="btn btn-link btn-xs panel-widget-reorder align-self-center pull-left js-stop-immediate-propagation"><i class="fa fa-bars"></i></a>
                        <h1><%# FormatAsHtmlTitle(Eval("Name").ToString()) %></h1>

                        <div class="action-wrapper">
                            <asp:HyperLink ID="hlShowMoreAttributes" runat="server" CssClass="action js-show-more-family-attributes"><i class="fa fa-chevron-down"></i></asp:HyperLink>
                            <asp:HyperLink ID="hlEditGroup" runat="server" AccessKey="O" ToolTip="Alt+O" CssClass="action"><i class="fa fa-pencil"></i></asp:HyperLink>
                        </div>
                    </header>

                    <asp:Literal ID="lGroupHeader" runat="server" />

                    <div class="row group-details">
                        <div class="col-md-8 clearfix">
                            <ul class="groupmembers">
                                <asp:Repeater ID="rptrMembers" runat="server">
                                    <ItemTemplate>
                                        <li class='<%# FormatPersonCssClass( (bool)Eval("Person.IsDeceased") ) %>'>
                                            <a href='<%# FormatPersonLink(Eval("PersonId").ToString()) %>'>
                                                <div class="person-image" id="divPersonImage" runat="server"></div>
                                                <div class="person-info">
                                                    <h4><%# FormatPersonName(Eval("Person.NickName") as string, Eval("Person.LastName") as string) %></h4>
                                                    <small><%# FormatPersonDetails( Container.DataItem ) %></small>
                                                </div>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                        <div class="col-md-4 addresses clearfix">
                            <ul class="list-unstyled margin-t-md">
                                <asp:Repeater ID="rptrAddresses" runat="server">
                                    <ItemTemplate>
                                        <li class="address rollover-container clearfix">
                                            <h4><%# FormatAddressType(Eval("GroupLocationTypeValue.Value")) %></h4>
                                            <a id="aMap" runat="server" title="Map This Address" class="map" target="_blank">
                                                <i class="fa fa-map-marker"></i>
                                            </a>
                                            <div class="address">
                                                <%# FormatAddress(Eval("Location")) %>
                                            </div>
                                            <div class="pull-left rollover-item">
                                                <asp:LinkButton ID="lbVerify" runat="server" CommandName="verify" ToolTip="Verify Address">
                                                    <i class="fa fa-globe"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lbLocationSettings" runat="server" CommandName="settings" ToolTip="Configure Location">
                                                    <i class="fa fa-gear"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                    </div>

                    <asp:panel ID="pnlGroupAttributes" runat="server" CssClass="margin-l-md js-group-attributes" style="min-height: 22px;" >
                        <div class="row">
                            <asp:PlaceHolder ID="phGroupAttributes" runat="server" />
                        </div>
                        <div class="row js-more-group-attributes" style="display:none">
                            <asp:PlaceHolder ID="phMoreGroupAttributes" runat="server" />
                        </div>
                    </asp:panel>

                    <asp:Literal ID="lGroupFooter" runat="server" />

                </asp:Panel>

            </ItemTemplate>
        </asp:Repeater>

        </div>

        <script>
            Sys.Application.add_load(function () {

                var fixHelper = function (e, ui) {
                    ui.children().each(function () {
                        $(this).width($(this).width());
                    });
                    return ui;
                };

                // javascript to make the Reorder buttons work on the panel-widget controls
                $('.js-grouplist-sort-container').sortable({
                    helper: fixHelper,
                    handle: '.panel-widget-reorder',
                    containment: 'parent',
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        {
                            var start_pos = ui.item.index();
                            ui.item.data('start_pos', start_pos);
                        }
                    },
                    update: function (event, ui) {
                        {
                            var newItemIndex = $(ui.item).prevAll('.panel-widget').length;
                            var postbackArg = 're-order-panel-widget:' + ui.item.attr('id') + ';' + newItemIndex;
                            window.location = "javascript:__doPostBack('<%=upGroupMembers.ClientID %>', '" +  postbackArg + "')";
                        }
                    }
                });
            });
        </script>

    </ContentTemplate>
</asp:UpdatePanel>
