<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Solution.UI.Web.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/klevu-landing-page-style.css" rel="stylesheet" type="text/css" />
    <link href="/css/klevu-landing-responsive.css" rel="stylesheet" type="text/css" />
    <%-- <script> var klevu_apiKey = 'klevu-13939350527231', klevu_analytics_key = 'klevu-139393549851193', searchTextBoxName = 'txtSearch', klevu_lang = 'en', klevu_result_top_margin = '', klevu_result_left_margin = ''; (function () { var ws = document.createElement('script'); ws.type = 'text/javascript'; ws.async = true; ws.src = 'https://box.klevu.com/klevu-js-v1/js/klevu-webstore.js'; var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ws, s); })(); </script>--%>
    <div class="breadcrumbs">
        <a title="Home" href="/">Home </a>
        <img src="/images/spacer.png" alt="" title="" class="breadcrumbs-bullet-icon"><span>
            Search</span>
    </div>
    <div class="content-main">
        <div class="kuContainer">
            <div class="kuProListing">
                <div class="kuNoRecordFound" id="kuNoRecordFound" style="display: none;">
                    <p>No matching products found for "red awef"</p>
                </div>
                <div class="kuFilters" id="kuFilters">
                </div>
                <!-- End of kuFilters -->

                <div class="kuResultList" id="kuResultListBlock">
                    <div class="kuListHeader">
                        <div class="kuTotResults" id="kuTotResults"></div>
                        <div>
                            <div class="kuSortby">
                                <label>Sort by:</label>
                                <select name="kuSortby" id="kuSortby" onchange="klevu_changeSortingOptionsForLandigPage(this.value);">
                                    <option value="rel">Relevance</option>
                                    <option value="lth">Price: Low to high</option>
                                    <option value="htl">Price: High to low</option>
                                </select>
                            </div>

                            <div class="kuView">
                                <a class="kuGridviewBtn" id="gridViewBtn" onclick="setKuView('grid');">
                                    <span class="icon-gridview"></span>
                                </a>
                                <a class="kuListviewBtn kuCurrent" id="listViewBtn" onclick="setKuView('list');">
                                    <span class="icon-listview"></span>
                                </a>
                            </div>

                            <div class="kuPerPage">
                                <label>Items per page:</label>
                                <select onchange="klevu_changeItemsPerPage(this.value);" id="noOfRecords1">
                                    <option>12</option>
                                    <option>24</option>
                                    <option>36</option>
                                </select>
                            </div>

                            <div class="kuPagination" id="kuPagination1">
                            </div>

                            <div class="kuClearLeft"></div>
                        </div>

                    </div>


                    <div id="loader" style="text-align: center">
                        <img src='img/ku-loader.gif' />
                    </div>
                    <div class="kuListView" id="kuResultsView">
                    </div>
                    <!-- End of klevuResults-->
                    <div class="kuBottomPagi">
                        <div class="kuPerPage">
                            <label>Items per page:</label>
                            <select onchange="klevu_changeItemsPerPage(this.value);" id="noOfRecords2">
                                <option>12</option>
                                <option>24</option>
                                <option>36</option>
                            </select>
                        </div>
                        <div class="kuPagination" id="kuPagination2">
                        </div>
                        <div class="kuClearLeft"></div>
                    </div>


                </div>
                <div class="klevu-clear-both"></div>
            </div>
            <!-- End of kuProListing -->
        </div>
        <!-- End of klevu-container -->
        <input type="hidden" name="noOfRecords" id="noOfRecords" value="12" />
        <input type="hidden" name="startPos" id="startPos" value="0" />
        <input type="hidden" name="totalResultsFound" id="totalResultsFound" value="0" />
        <input type="hidden" name="searchedKeyword" id="searchedKeyword" value="" />
        <input type="hidden" name="totalPages" id="totalPages" value="0" />
        <script src="https://box.klevu.com/klevu-js-v1/js-1-1/klevu-landing.js"></script>
        <script>
            document.getElementById('searchedKeyword').value = klevu_getParamValue("searchterm"); </script>
    </div>
</asp:Content>
