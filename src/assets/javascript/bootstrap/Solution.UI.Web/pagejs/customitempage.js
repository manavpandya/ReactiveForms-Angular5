var selectedname = '';
function fabriccategory() {

    var windowHeight = 0;
    windowHeight = $('#ContentPlaceHolder1_divcustomfabric').height(); //window.innerHeight;

    document.getElementById('prepage').style.height = windowHeight + 'px';
    document.getElementById('prepage').style.display = '';


    getCustompage(0, document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString());



}

function getoptionvalue(fnname, cntrlid, selectId) {

    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/" + fnname,
                  data: "{strFabriccategoryId: '" + document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString() + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {
                      document.getElementById(cntrlid).options[0] = null;

                      var alldata = msg.d.toString().split(',');
                      if (alldata.length > 0) {
                          document.getElementById(cntrlid).length = alldata.length;

                          for (var i = 0; i < alldata.length; i++) {
                              document.getElementById(cntrlid).options[i].text = alldata[i].toString();
                              document.getElementById(cntrlid).options[i].value = alldata[i].toString();
                          }
                          if (selectId == 'ddlcustomcolor') {
                              document.getElementById(selectId).innerHTML = 'Color';
                          }
                          else if (selectId == 'ddlcustompattern') {
                              document.getElementById(selectId).innerHTML = 'Pattern';
                          }
                          else if (selectId == 'ddlcustomfabri') {
                              document.getElementById(selectId).innerHTML = 'Fabric';
                          }
                      }

                  },
                  Error: function (x, e) {
                  }
              });
}


function fabriccategorycomb() {

    var windowHeight = 0;
    windowHeight = $('#ContentPlaceHolder1_divcustomfabric').height(); //window.innerHeight;

    document.getElementById('prepage').style.height = windowHeight + 'px';
    document.getElementById('prepage').style.display = '';

    getCustompagedrop(0, document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString());

    getoptionvalue('getCustomfabric', 'ContentPlaceHolder1_ddlfabric', 'ddlcustomfabri');
    getoptionvalue('getCustompattern', 'ContentPlaceHolder1_ddlcustompattern', 'ddlcustompattern');
    getoptionvalue('getCustomcolor', 'ContentPlaceHolder1_ddlcustomcolor', 'ddlcustomcolor');

}
function updateimage(pid) {

    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/getCustompageimage",
                  data: "{strProductId: '" + pid + "'}",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {
                      if (document.getElementById('ContentPlaceHolder1_imgMain') != null) {
                          $('#ContentPlaceHolder1_imgMain').removeAttr('data-zoom-image');
                                                    document.getElementById('ContentPlaceHolder1_imgMain').src = msg.d;
                          $('#ContentPlaceHolder1_imgMain').attr('data-zoom-image', msg.d.toString().replace('/medium/','/large/').replace('/Medium/','/large/'));
                          document.getElementById('ContentPlaceHolder1_imgMain').onclick = function () { ShowModelHelp(pid); }

                      }
                  },
                  Error: function (x, e) {
                  }
              });
}
function selectswatchimage(id, aid, fbname) {

    if (document.getElementById('HdnPriceFb') != null) {
        document.getElementById('HdnPriceFb').value = id;
    }

    var allli = document.getElementById('ulfabricall1').getElementsByTagName('a');

    for (var iS = 0; iS < allli.length; iS++) {
        var eltSelect = allli[iS];

        if (eltSelect.id == aid) {
            var ii = aid.replace('aswatch', 'liswatch');
            document.getElementById(ii).className = 'active';
        }
        else {
            var ii = eltSelect.id.replace('aswatch', 'liswatch');

            document.getElementById(ii).className = '';
        }
    }

    getallcolorswatch(document.getElementById('hdncolorvalue').value, id);
    if (document.getElementById('fabricnamestrg') != null) {
        document.getElementById('fabricnamestrg').innerHTML = fbname;
    }

}
function getallcolorswatch(id, pid) {
    var afabric = '';
    var apattern = '';
    var acolor = '';



    if (document.getElementById('ContentPlaceHolder1_ddlfabric') != null) {
        afabric = document.getElementById('ContentPlaceHolder1_ddlfabric').options[document.getElementById('ContentPlaceHolder1_ddlfabric').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomcolor') != null) {
        acolor = document.getElementById('ContentPlaceHolder1_ddlcustomcolor').options[document.getElementById('ContentPlaceHolder1_ddlcustomcolor').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustompattern') != null) {
        apattern = document.getElementById('ContentPlaceHolder1_ddlcustompattern').options[document.getElementById('ContentPlaceHolder1_ddlcustompattern').selectedIndex].text.toString()
    }
    if (document.getElementById('hdncolorvalue') != null) {
        document.getElementById('hdncolorvalue').value = id;
    }

    if (document.getElementById('hdncolorvaluenext').value == '1') {
        id = parseInt(id) + parseInt(20);
    }
    else if (document.getElementById('hdncolorvaluepre').value == '1' && id == 21) {


    }
    else if (document.getElementById('hdncolorvaluepre').value == '1') {
        id = parseInt(id);
    }

    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/getCustompageswatch",
                  data: "{strProductId: '" + pid + "',strFabriccategoryId: '" + document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString() + "',fabric:'" + afabric + "',pattern:'" + apattern + "',color:'" + acolor + "',nextId: '" + id + "',Previousid: '0' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {

                      if (document.getElementById('ContentPlaceHolder1_divcustomfabric') != null) {

                          $('#otherproduct').html(msg.d);

                          if (document.getElementById('addtocartdivnew') != null) {
                              $('#addtocartdivnew').html('');

                          }
                          varianttabhideshowcustom(0);
                          if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                              document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(pid, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                          }

                          if (msg.d.toString().indexOf('~~h-') > -1) {
                              var ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~h-'), msg.d.toString().indexOf('-h~~')).replace('~~h-', '');
                              updateimage(ptID);
                              ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~p-'), msg.d.toString().indexOf('-p~~')).replace('~~p-', '');

                              if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                                  document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(ptID, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                              }
                              document.getElementById('prepage').style.display = 'none';
                              //$('#ContentPlaceHolder1_divcustomfabric').fadeIn(2000);

                          }


                          var ntID = msg.d.toString().substring(msg.d.toString().indexOf('~~n-'), msg.d.toString().indexOf('-n~~')).replace('~~n-', '');

                          var prtID = msg.d.toString().substring(msg.d.toString().indexOf('~~pr-'), msg.d.toString().indexOf('-pr~~')).replace('~~pr-', '');

                          if (document.getElementById('anextfabric') != null && ntID != '0') {

                              $('#anextfabric').removeAttr("onclick");
                              // $('#anextfabric').attr("onclick", "getallcolor(" + ntID + ");");
                              document.getElementById('anextfabric').onclick = function () { getallcolor(ntID); }
                              document.getElementById('anextfabric').style.display = 'none';
                          }
                          else {
                              if (document.getElementById('anextfabric') != null) {
                                  document.getElementById('anextfabric').style.display = 'none';
                              }
                          }
                          if (document.getElementById('apreviousfabric') != null && parseInt(prtID) > parseInt(20)) {
                              $('#apreviousfabric').removeAttr("onclick");

                              document.getElementById('apreviousfabric').onclick = function () { getallcolorpre(prtID); }
                              document.getElementById('apreviousfabric').style.display = 'none';
                          }
                          else {
                              if (document.getElementById('apreviousfabric') != null) {
                                  document.getElementById('apreviousfabric').style.display = 'none';
                              }
                          }


                      }
                  },
                  Error: function (x, e) {
                  }
              });
}
var flag = 0;
function getallcolor(id) {

    $('#divscroll').scroll(function () { });
    if (document.getElementById('divscrollloader') != null) {
        document.getElementById('divscrollloader').style.display = '';
    }
    var afabric = '';
    var apattern = '';
    var acolor = '';
    if (document.getElementById('ContentPlaceHolder1_ddlfabric') != null) {
        afabric = document.getElementById('ContentPlaceHolder1_ddlfabric').options[document.getElementById('ContentPlaceHolder1_ddlfabric').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomcolor') != null) {
        acolor = document.getElementById('ContentPlaceHolder1_ddlcustomcolor').options[document.getElementById('ContentPlaceHolder1_ddlcustomcolor').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustompattern') != null) {
        apattern = document.getElementById('ContentPlaceHolder1_ddlcustompattern').options[document.getElementById('ContentPlaceHolder1_ddlcustompattern').selectedIndex].text.toString()
    }
    if (document.getElementById('hdncolorvalue') != null) {
        document.getElementById('hdncolorvalue').value = id;
    }
    document.getElementById('hdncolorvaluenext').value = '1';
    document.getElementById('hdncolorvaluepre').value = '0';



    var dscrolvalue = $("#divscroll").scrollTop();

    flag = 0;
    if (flag == 0) {
        flag = 1;

        $.ajax(
                  {
                      type: "POST",
                      url: "/TestMail.aspx/getCustompagecolorall",
                      data: "{strProductId: '0',strFabriccategoryId: '" + document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString() + "',fabric:'" + afabric + "',pattern:'" + apattern + "',color:'" + acolor + "',nextId: '" + id + "',Previousid: '0' }",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      async: "true",
                      cache: "false",
                      success: function (msg) {

                          if (document.getElementById('ContentPlaceHolder1_divcustomfabric') != null) {



                              $('#ulfabricall1').append(msg.d);


                              $('#divscroll').attr("style", "height: 450px; overflow-y:auto;");

                              var ntID = msg.d.toString().substring(msg.d.toString().indexOf('~~n-'), msg.d.toString().indexOf('-n~~')).replace('~~n-', '');

                              var prtID = msg.d.toString().substring(msg.d.toString().indexOf('~~pr-'), msg.d.toString().indexOf('-pr~~')).replace('~~pr-', '');




                              if (document.getElementById('anextfabric') != null && ntID != '0') {

                                  var totlahieght = parseInt(ntID) / 20;
                                  totlahieght = totlahieght.toFixed(0);

                                  $('#anextfabric').removeAttr("onclick");

                                  document.getElementById('divscroll').onscroll = function () { if ($(this)[0].scrollHeight - $(this).scrollTop() == $(this).outerHeight()) { getallcolor(ntID); } }
                                  document.getElementById('anextfabric').style.display = 'none';
                                  $('#divscroll').scrollTop(dscrolvalue);


                              }
                              else {
                                  if (document.getElementById('anextfabric') != null) {
                                      document.getElementById('anextfabric').style.display = 'none';
                                  }
                                  document.getElementById('divscroll').onscroll = function () { }
                              }
                              if (document.getElementById('apreviousfabric') != null && parseInt(prtID) > parseInt(20)) {
                                  $('#apreviousfabric').removeAttr("onclick");

                                  document.getElementById('apreviousfabric').onclick = function () { getallcolorpre(prtID); }
                                  document.getElementById('apreviousfabric').style.display = 'none';
                              }
                              else {
                                  if (document.getElementById('apreviousfabric') != null) {
                                      document.getElementById('apreviousfabric').style.display = 'none';
                                  }
                              }
                              varianttabhideshowcustom(0);
                              document.getElementById('divscrollloader').style.display = 'none';

                          }
                      },
                      Error: function (x, e) {
                      }
                  });
    }
}
function getallcolorpre(id) {
    var afabric = '';
    var apattern = '';
    var acolor = '';
    if (document.getElementById('ContentPlaceHolder1_ddlfabric') != null) {
        afabric = document.getElementById('ContentPlaceHolder1_ddlfabric').options[document.getElementById('ContentPlaceHolder1_ddlfabric').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomcolor') != null) {
        acolor = document.getElementById('ContentPlaceHolder1_ddlcustomcolor').options[document.getElementById('ContentPlaceHolder1_ddlcustomcolor').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustompattern') != null) {
        apattern = document.getElementById('ContentPlaceHolder1_ddlcustompattern').options[document.getElementById('ContentPlaceHolder1_ddlcustompattern').selectedIndex].text.toString()
    }

    if (document.getElementById('hdncolorvalue') != null) {
        document.getElementById('hdncolorvalue').value = id;
    }

    document.getElementById('hdncolorvaluenext').value = '0';
    document.getElementById('hdncolorvaluepre').value = '1';
    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/getCustompagecolorall",
                  data: "{strProductId: '0',strFabriccategoryId: '" + document.getElementById('ContentPlaceHolder1_ddlfabriccategory').options[document.getElementById('ContentPlaceHolder1_ddlfabriccategory').selectedIndex].text.toString() + "',fabric:'" + afabric + "',pattern:'" + apattern + "',color:'" + acolor + "',nextId: '0',Previousid: '" + id + "' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {

                      if (document.getElementById('ContentPlaceHolder1_divcustomfabric') != null) {

                          $('#ulfabricall').html(msg.d);

                          varianttabhideshowcustom(0);
                          document.getElementById('prepage').style.display = 'none';

                          var ntID = msg.d.toString().substring(msg.d.toString().indexOf('~~n-'), msg.d.toString().indexOf('-n~~')).replace('~~n-', '');

                          var prtID = msg.d.toString().substring(msg.d.toString().indexOf('~~pr-'), msg.d.toString().indexOf('-pr~~')).replace('~~pr-', '');

                          if (document.getElementById('anextfabric') != null && ntID != '0') {

                              $('#anextfabric').removeAttr("onclick");

                              document.getElementById('anextfabric').onclick = function () { getallcolor(ntID); }
                              document.getElementById('anextfabric').style.display = '';
                          }
                          else {
                              if (document.getElementById('anextfabric') != null) {
                                  document.getElementById('anextfabric').style.display = 'none';
                              }
                          }
                          if (document.getElementById('apreviousfabric') != null && parseInt(prtID) > parseInt(20)) {
                              $('#apreviousfabric').removeAttr("onclick");

                              document.getElementById('apreviousfabric').onclick = function () { getallcolorpre(prtID); }
                              document.getElementById('apreviousfabric').style.display = '';
                          }
                          else {
                              if (document.getElementById('apreviousfabric') != null) {
                                  document.getElementById('apreviousfabric').style.display = 'none';
                              }
                          }

                      }
                  },
                  Error: function (x, e) {
                  }
              });
}

function getCustompagedrop(pid, fId) {

    var afabric = '';
    var apattern = '';
    var acolor = '';


    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/getCustompage",
                  data: "{strProductId: '" + pid + "',strFabriccategoryId: '" + fId + "',fabric:'" + afabric + "',pattern:'" + apattern + "',color:'" + acolor + "' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {

                      if (document.getElementById('ContentPlaceHolder1_divcustomfabric') != null) {

                          $('#ContentPlaceHolder1_divcustomfabric').html(msg.d);




                          varianttabhideshowcustom(0);
                          if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                              document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(pid, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                          }

                          if (msg.d.toString().indexOf('~~h-') > -1) {
                              var ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~h-'), msg.d.toString().indexOf('-h~~')).replace('~~h-', '');
                              updateimage(ptID);
                              ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~p-'), msg.d.toString().indexOf('-p~~')).replace('~~p-', '');
                              if (ptID == "" || ptID == "0") {
                                  if (document.getElementById('ContentPlaceHolder1_divswatchimage') != null) {
                                      document.getElementById('ContentPlaceHolder1_divswatchimage').style.display = 'none';
                                  }
                              }
                              else {
                                  if (document.getElementById('ContentPlaceHolder1_divswatchimage') != null) {
                                      document.getElementById('ContentPlaceHolder1_divswatchimage').style.display = '';
                                  }
                              }
                              if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                                  document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(ptID, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                              }
                              document.getElementById('prepage').style.display = 'none';

                              if (document.getElementById('anextfabric') != null) {
                                  document.getElementById('anextfabric').style.display = 'none';
                              }
                              document.getElementById('divscroll').onscroll = function () { if ($(this)[0].scrollHeight - $(this).scrollTop() == $(this).outerHeight()) { getallcolor(21); } }

                          }

                          document.getElementById('hdncolorvalue').value = "1";



                      }
                  },
                  Error: function (x, e) {
                  }
              });
}

function getCustompage(pid, fId) {

    var afabric = '';
    var apattern = '';
    var acolor = '';
    if (document.getElementById('ContentPlaceHolder1_ddlfabric') != null) {
        afabric = document.getElementById('ContentPlaceHolder1_ddlfabric').options[document.getElementById('ContentPlaceHolder1_ddlfabric').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomcolor') != null) {
        acolor = document.getElementById('ContentPlaceHolder1_ddlcustomcolor').options[document.getElementById('ContentPlaceHolder1_ddlcustomcolor').selectedIndex].text.toString()
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustompattern') != null) {
        apattern = document.getElementById('ContentPlaceHolder1_ddlcustompattern').options[document.getElementById('ContentPlaceHolder1_ddlcustompattern').selectedIndex].text.toString()
    }

    $.ajax(
              {
                  type: "POST",
                  url: "/TestMail.aspx/getCustompage",
                  data: "{strProductId: '" + pid + "',strFabriccategoryId: '" + fId + "',fabric:'" + afabric + "',pattern:'" + apattern + "',color:'" + acolor + "' }",
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  async: "true",
                  cache: "false",
                  success: function (msg) {

                      if (document.getElementById('ContentPlaceHolder1_divcustomfabric') != null) {

                          $('#ContentPlaceHolder1_divcustomfabric').html(msg.d);




                          varianttabhideshowcustom(0);
                          if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                              document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(pid, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                          }

                          if (msg.d.toString().indexOf('~~h-') > -1) {
                              var ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~h-'), msg.d.toString().indexOf('-h~~')).replace('~~h-', '');
                              updateimage(ptID);

                              ptID = msg.d.toString().substring(msg.d.toString().indexOf('~~p-'), msg.d.toString().indexOf('-p~~')).replace('~~p-', '');
                              if (ptID == "" || ptID == "0") {
                                  if (document.getElementById('ContentPlaceHolder1_divswatchimage') != null) {
                                      document.getElementById('ContentPlaceHolder1_divswatchimage').style.display = 'none';
                                  }
                              }
                              else {
                                  if (document.getElementById('ContentPlaceHolder1_divswatchimage') != null) {
                                      document.getElementById('ContentPlaceHolder1_divswatchimage').style.display = '';
                                  }
                              }
                              if (document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch') != null) {
                                  document.getElementById('ContentPlaceHolder1_btnAddTocartSwatch').onclick = function () { InsertProductSwatch(ptID, 'ContentPlaceHolder1_btnAddTocartSwatch'); return false; }
                              }
                              document.getElementById('prepage').style.display = 'none';

                              if (document.getElementById('anextfabric') != null) {
                                  document.getElementById('anextfabric').style.display = 'none';
                              }
                              document.getElementById('divscroll').onscroll = function () { if ($(this)[0].scrollHeight - $(this).scrollTop() == $(this).outerHeight()) { getallcolor(21); } }

                          }

                          document.getElementById('hdncolorvalue').value = "1";



                      }
                  },
                  Error: function (x, e) {
                  }
              });
}

function ReviewCount(RevId, mode) {
    if (RevId > 0) {
        if (document.getElementById('ContentPlaceHolder1_hdnReviewType') != null) {
            document.getElementById('ContentPlaceHolder1_hdnReviewType').value = mode;
            document.getElementById('ContentPlaceHolder1_hdnReviewId').value = RevId;
            document.getElementById('ContentPlaceHolder1_btnReviewCount').click();
        }
    }
}
function changeqtyinlist(id) {
    if (document.getElementById('ddlqtymain') != null) {
        document.getElementById('ddlqtymain').options[0] = null;
        if (id <= 0) {
            id = 0;
            document.getElementById('ddlqtymain').length = 1;
            document.getElementById('ddlqtymain').options[0].text = "0";
            document.getElementById('ddlqtymain').options[0].value = "0";
            document.getElementById('ContentPlaceHolder1_txtQty').value = "0";
            document.getElementById('txtqty-main').innerHTML = "0";


        }
        else {
            if (id > 25) {
                document.getElementById('ddlqtymain').length = 25;
            }
            else {
                document.getElementById('ddlqtymain').length = id;
            }
            for (var i = 1; i <= id; i++) {
                if (i <= 25) {
                    document.getElementById('ddlqtymain').options[i - 1].text = i.toString();
                    document.getElementById('ddlqtymain').options[i - 1].value = i.toString();
                }
            }
            document.getElementById('ContentPlaceHolder1_txtQty').value = "1";
            document.getElementById('ddlqtymain').selectedIndex = 0;
            document.getElementById('txtqty-main').innerHTML = "1";
        }
    }

}

function changeqtyonselection() {
    if (document.getElementById('ddlqtymain') != null) {
        if (document.getElementById('txtqty-main') != null) {
            document.getElementById('txtqty-main').innerHTML = document.getElementById('ddlqtymain').options[document.getElementById('ddlqtymain').selectedIndex].text;
            document.getElementById('ContentPlaceHolder1_txtQty').value = document.getElementById('ddlqtymain').options[document.getElementById('ddlqtymain').selectedIndex].text;
            PriceChangeondropdown();
        }
    }
}

function varianttabhideshowcustom(id) {
    var allselect = document.getElementById('ContentPlaceHolder1_divcustomfabric').getElementsByTagName('div');

    for (var iS = 0; iS < allselect.length; iS++) {
        var eltSelect = allselect[iS];
        if (eltSelect.id.toString().indexOf('divcolspancustom-') > -1) {
            if (eltSelect.id.toString().replace('divcolspancustom-', '') == id) {
                if (document.getElementById('divcolspancustomvalue-' + id).style.display == 'none') {

                    $('#divcolspancustomvalue-' + id).slideToggle();
                    if (document.getElementById('spancolspancustomvalue-' + id) != null) {
                        document.getElementById('spancolspancustomvalue-' + id).style.display = '';

                    }
                    eltSelect.className = 'readymade-detail-pt1-pro active';

                }
                else {
                    if (eltSelect.id.toString().replace('divcolspancustom-', '') != id) {
                        $('#divcolspancustomvalue-' + id).slideToggle();
                        if (document.getElementById('spancolspancustomvalue-' + id) != null) {
                            document.getElementById('spancolspancustomvalue-' + id).style.display = 'none';
                        }
                        eltSelect.className = 'readymade-detail-pt1-pro';
                    }


                }


            }
            else {
                eltSelect.className = 'readymade-detail-pt1-pro';
                if (document.getElementById('spancolspancustomvalue-' + eltSelect.id.toString().replace('divcolspancustom-', '')) != null) {
                    document.getElementById('spancolspancustomvalue-' + eltSelect.id.toString().replace('divcolspancustom-', '')).style.display = 'none';
                }

                document.getElementById('divcolspancustomvalue-' + eltSelect.id.toString().replace('divcolspancustom-', '')).style.display = 'none';
            }
        }

    }

}
function varianttabhideshow(id) {

    var allselect = document.getElementById('divVariant').getElementsByTagName('div');

    for (var iS = 0; iS < allselect.length; iS++) {
        var eltSelect = allselect[iS];
        if (eltSelect.id.toString().indexOf('divcolspan-') > -1) {
            if (eltSelect.id.toString().replace('divcolspan-', '') == id) {
                if (document.getElementById('divSelectvariant-' + id).style.display == 'none') {

                    $('#divSelectvariant-' + id).slideToggle();
                    eltSelect.className = 'readymade-detail-pt1-pro active';
                }
                else {
                    if (eltSelect.id.toString().replace('divcolspan-', '') != id) {
                        $('#divSelectvariant-' + id).slideToggle();
                        eltSelect.className = 'readymade-detail-pt1-pro';
                    }


                }


            }
            else {
                eltSelect.className = 'readymade-detail-pt1-pro';
                document.getElementById('divSelectvariant-' + eltSelect.id.toString().replace('divcolspan-', '')).style.display = 'none';
            }
        }

    }

}
function tabdisplaycartpolicy(id, id1) {

    if (document.getElementById('lishippingtime') != null) {
        document.getElementById('lishippingtime').className = '';
    }
    if (document.getElementById('divshippingtime') != null) {
        document.getElementById('divshippingtime').style.display = 'none';
    }

    if (document.getElementById('lireturnpolicy') != null) {
        document.getElementById('lireturnpolicy').className = '';
    }
    if (document.getElementById('divreturnpolicy') != null) {
        document.getElementById('divreturnpolicy').style.display = 'none';
    }


    if (document.getElementById(id) != null) {
        document.getElementById(id).className = 'tabberactive';
    }
    if (document.getElementById(id1) != null) {
        document.getElementById(id1).style.display = '';
    }



}
function tabdisplaycart(id, id1) {

    if (document.getElementById('ContentPlaceHolder1_liswatch') != null) {
        document.getElementById('ContentPlaceHolder1_liswatch').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divswatch') != null) {
        document.getElementById('ContentPlaceHolder1_divswatch').style.display = 'none';
    }

    if (document.getElementById('ContentPlaceHolder1_licustom') != null) {
        document.getElementById('ContentPlaceHolder1_licustom').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divcustom') != null) {
        document.getElementById('ContentPlaceHolder1_divcustom').style.display = 'none';
    }

    if (document.getElementById('ContentPlaceHolder1_liready') != null) {
        document.getElementById('ContentPlaceHolder1_liready').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divready') != null) {
        document.getElementById('ContentPlaceHolder1_divready').style.display = 'none';
    }
    if (document.getElementById('ContentPlaceHolder1_licoloroption') != null) {
        document.getElementById('ContentPlaceHolder1_licoloroption').className = '';
    }
    if (document.getElementById('ContentPlaceHolder1_divColorOption') != null) {
        document.getElementById('ContentPlaceHolder1_divColorOption').style.display = 'none';
    }

    if (document.getElementById(id) != null) {
        document.getElementById(id).className = 'tabberactive';
    }
    if (document.getElementById(id1) != null) {
        document.getElementById(id1).style.display = '';
    }



}


function ShowModelForPleatGuidequote() {

    document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
    document.getElementById('Iframepricequote').height = '600px';
    document.getElementById('Iframepricequote').width = '970px';
    document.getElementById('popupContactpricequote').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContactpricequote').style.width = '970px';
    document.getElementById('popupContactpricequote').style.height = '600px';
    window.scrollTo(0, 0);
    centerPopup1pricequote(); loadPopup1pricequote(); //load popup here 
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divPleatGuide').innerHTML;
}

function variantDetailpricequote(divid) {

    document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
    document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '';
    document.getElementById('Iframepricequote').height = '500px';
    document.getElementById('Iframepricequote').width = '517px';
    document.getElementById('popupContactpricequote').setAttribute("style", "z-index: 2000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContactpricequote').style.width = '517px';
    document.getElementById('popupContactpricequote').style.height = '500px';

    window.scrollTo(0, 0);
    centerPopup1pricequote(); loadPopup1pricequote(); //load popup here 
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("Iframepricequote").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById(divid).innerHTML;
}


function ShowModelForPriceQuote() {


    document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay').height = '260px';
    document.getElementById('frmdisplay').width = '550px';
    document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:550px;height:260px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact').style.width = '550px';
    document.getElementById('popupContact').style.height = '260px';
    window.scrollTo(0, 0);

    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('ContentPlaceHolder1_divPriceQuote').innerHTML.toString().replace(/ContentPlaceHolder1_/g, '');

    centerPopup();
    loadPopup();
}

function priceQuoteDiff() {
    var ifr = document.getElementById("frmdisplay");

    ShowModelHelpShipping('/CustomQuoteSize.aspx?ProductId=' + document.getElementById("hdnProductID").value);
    return true;

}
function priceQuote() {
    var ifr = document.getElementById("frmdisplay");


    if ((ifr.contentWindow.document.getElementById('ddlHeaderDesign').options[ifr.contentWindow.document.getElementById('ddlHeaderDesign').selectedIndex]).text == 'Select One') {
        alert('Please select Header.');
        ifr.contentWindow.document.getElementById('ddlHeaderDesign').focus();
        return false;
    }
    if ((ifr.contentWindow.document.getElementById('ddlFinishedWidth').options[ifr.contentWindow.document.getElementById('ddlFinishedWidth').selectedIndex]).text == 'Width') {

        alert('Please select Width.');
        ifr.contentWindow.document.getElementById('ddlFinishedWidth').focus();
        return false;
    }
    if ((ifr.contentWindow.document.getElementById('ddlFinishedLength').options[ifr.contentWindow.document.getElementById('ddlFinishedLength').selectedIndex]).text == 'Length') {

        alert('Please select Length.');
        ifr.contentWindow.document.getElementById('ddlFinishedLength').focus();
        return false;
    }
    if ((ifr.contentWindow.document.getElementById('ddlOptionType').options[ifr.contentWindow.document.getElementById('ddlOptionType').selectedIndex]).text == 'Options') {

        alert('Please select Options.');
        ifr.contentWindow.document.getElementById('ddlOptionType').focus();
        return false;
    }
    if ((ifr.contentWindow.document.getElementById('ddlQuantity').options[ifr.contentWindow.document.getElementById('ddlQuantity').selectedIndex]).text == 'Quantity') {

        alert('Please select Quantity.');
        ifr.contentWindow.document.getElementById('ddlQuantity').focus();
        return false;
    }

    document.getElementById('ContentPlaceHolder1_hdnheaderqoute').value = (ifr.contentWindow.document.getElementById('ddlHeaderDesign').options[ifr.contentWindow.document.getElementById('ddlHeaderDesign').selectedIndex]).value;
    document.getElementById('ContentPlaceHolder1_hdnwidthqoute').value = (ifr.contentWindow.document.getElementById('ddlFinishedWidth').options[ifr.contentWindow.document.getElementById('ddlFinishedWidth').selectedIndex]).value;
    document.getElementById('ContentPlaceHolder1_hdnlengthqoute').value = (ifr.contentWindow.document.getElementById('ddlFinishedLength').options[ifr.contentWindow.document.getElementById('ddlFinishedLength').selectedIndex]).value;
    document.getElementById('ContentPlaceHolder1_hdnoptionhqoute').value = (ifr.contentWindow.document.getElementById('ddlOptionType').options[ifr.contentWindow.document.getElementById('ddlOptionType').selectedIndex]).value;
    document.getElementById('ContentPlaceHolder1_hdnquantityqoute').value = (ifr.contentWindow.document.getElementById('ddlQuantity').options[ifr.contentWindow.document.getElementById('ddlQuantity').selectedIndex]).value;

    disablePopup();
    ShowModelHelpShipping('/CustomQuoteSize.aspx?ProductId=' + document.getElementById("hdnProductID").value);
    return true;

}

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(beginRequest);

function beginRequest() {
    prm._scrollPosition = null;
}

function ShowModelHelpShipping(id) {

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '620px';
    document.getElementById('frmdisplay1').width = '830px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:830px;height:620px;");
    document.getElementById('popupContact1').style.width = '830px';
    document.getElementById('popupContact1').style.height = '620px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}

function PriceChangeondropdown() {
    var price = document.getElementById('ContentPlaceHolder1_hdnActual').value;
    var saleprice = document.getElementById('ContentPlaceHolder1_hdnprice').value;
    if (parseFloat(saleprice) == parseFloat(0)) {
        saleprice = price;
    }
    var vprice = 0;
    var isselected = 0;
    if (document.getElementById('diestimatedate') != null) {
        $('#diestimatedate').html('');

    }

    if (document.getElementById('divVariant')) {
        var allselect = document.getElementById('divVariant').getElementsByTagName('select');

        for (var iS = 0; iS < allselect.length; iS++) {
            var eltSelect = allselect[iS];
            var valsel = eltSelect.id.replace('Selectvariant-', 'divselvalue-');

            if (eltSelect.selectedIndex != 0) {

                var valsel1 = eltSelect.id.replace('Selectvariant-', 'divcolspan-');
                if (eltSelect.options[0].text.toLowerCase().indexOf('header design') > -1) {

                    if (eltSelect.options[eltSelect.selectedIndex].text.toString().indexOf('($') > -1) {
                        document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text.toLowerCase().replace('select ', '').replace('select', '') + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text.toString().substring(0, eltSelect.options[eltSelect.selectedIndex].text.toString().indexOf('($')) + '</strong>';
                    }
                    else {
                        document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text.toLowerCase().replace('select ', '').replace('select', '') + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text.toString() + '</strong>';
                    }
                }
                else {
                    document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text.toString().substring(0, eltSelect.options[eltSelect.selectedIndex].text.toString().indexOf('($')) + '</strong>';
                }
                //divselvalue
                //}
                if (eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').indexOf('($') > -1) {

                    var vtemp = eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').substring(eltSelect.options[eltSelect.selectedIndex].text.replace(/,/g, ' ').lastIndexOf('($') + 2);
                    vtemp = vtemp.replace(/\)/g, '');
                    vprice = parseFloat(vprice) + parseFloat(vtemp);
                }


            }
            else {
                if (document.getElementById(valsel) != null) {
                    document.getElementById(valsel).innerHTML = '';
                }
                isselected++;
            }
        }
    }
    if (parseFloat(vprice) > 0) {
        saleprice = parseFloat(vprice)
    }
    else {
        saleprice = parseFloat(saleprice) + parseFloat(vprice);
    }
    price = parseFloat(price);
    if (document.getElementById('ContentPlaceHolder1_divRegularPrice') != null) {
        document.getElementById('ContentPlaceHolder1_divRegularPrice').innerHTML = '<div class="listedprice-pro">Listed Price: <span>$' + price.toFixed(2) + '</span></div>';
    }
    var SalePriceTag;
    if (document.getElementById('ContentPlaceHolder1_hdnSalePriceTag') != null && document.getElementById('ContentPlaceHolder1_hdnSalePriceTag'.value) != '') {
        var SalePriceTag = ' <span>' + document.getElementById('ContentPlaceHolder1_hdnSalePriceTag').value + '</span>';
    }
    if (document.getElementById('ContentPlaceHolder1_divYourPrice') != null) {

        document.getElementById('ContentPlaceHolder1_divYourPrice').innerHTML = '$' + saleprice.toFixed(2) + '</strong>' + SalePriceTag;
    }

    if (document.getElementById('ContentPlaceHolder1_divYouSave') != null) {
        var diffprice = parseFloat(price) - parseFloat(saleprice);
        var diffpercentage = (parseFloat(diffprice) * parseFloat(100)) / parseFloat(price)

        document.getElementById('ContentPlaceHolder1_divYouSave').innerHTML = '<tt>You Save :</tt> <span style="color:#B92127;">$' + diffprice.toFixed(2) + '<span style="padding-left: 0px;color:#B92127;"> (' + diffpercentage.toFixed(2) + '%)</span></span>&nbsp;';
    }

    if (isselected == 0) {
        var qid = document.getElementById('ContentPlaceHolder1_txtQty').value;
        var pid = document.getElementById("hdnProductID").value;
        var Names = ""; var Values = "";
        if (document.getElementById('divVariant')) {
            var allselect = document.getElementById('divVariant').getElementsByTagName('select');

            for (var iS = 0; iS < allselect.length; iS++) {
                var eltSelect = allselect[iS];
                if (eltSelect.selectedIndex == 0) {
                    var valsel = dropid.replace('Selectvariant-', 'divselvalue-');
                    if (document.getElementById(valsel) != null) {
                        document.getElementById(valsel).innerHTML = '';
                    }
                }
                else {
                    var nametext = eltSelect.id.replace('Selectvariant-', 'divvariantname-');
                    Names = Names + document.getElementById(nametext).innerHTML + ',';
                    Values = Values + eltSelect.options[eltSelect.selectedIndex].text + ',';

                    var valsel = eltSelect.id.replace('Selectvariant-', 'divselvalue-');
                    if (document.getElementById(valsel) != null) {

                        var valsel1 = eltSelect.id.replace('Selectvariant-', 'divcolspan-');
                        if (eltSelect.options[eltSelect.selectedIndex].text.toString().indexOf('($') > -1) {
                            document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text.toString().substring(0, eltSelect.options[eltSelect.selectedIndex].text.toString().indexOf('($')) + '</strong>';
                        }
                        else {
                            document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + eltSelect.options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + eltSelect.options[eltSelect.selectedIndex].text.toString() + '</strong>';
                        }

                    }


                }
            }
        }
        $.ajax(
                {
                    type: "POST",
                    url: "/TestMail.aspx/GetDataAdminMessage",
                    data: "{ProductId: " + pid + ",ProductType: 1,Qty: " + qid + ",vValueid: '" + Values + "',vNameid: '" + Names + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (msg) {
                        if (document.getElementById('diestimatedate') != null) {
                            if (qid == 0) {
                                $('#diestimatedate').html('');
                                $('#diestimatedate').attr('style', 'display:block;');
                            }
                            else {
                                $('#diestimatedate').html('<label style="width: 100% !important;color:#B92127" class="readymade-detail-left">' + msg.d.replace('undefined', '') + '</label>');
                                $('#diestimatedate').attr('style', 'display:block;');
                            }
                        }
                    },
                    Error: function (x, e) {
                    }
                });
        $.ajax(
                                {
                                    type: "POST",
                                    url: "/TestMail.aspx/ChangeBackorderdate",
                                    data: "{VariantName: '" + Names + "',VariantValue: '" + Values + "', ProductID: " + pid + "}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: "true",
                                    cache: "false",
                                    success: function (msg) {
                                        if (document.getElementById('backorderdate') != null) {
                                            if (msg.d != null && msg.d != '' && msg.d != 'undefined') {

                                                $('#backorderdate').html('<label style="width: 100% !important;color:#B92127;font-weight:bold;" class="readymade-detail-left">Back Order</label>');
                                                $('#backorderdate').attr('style', 'display:block;');
                                            }
                                            else {
                                                $('#backorderdate').html('');
                                                $('#backorderdate').attr('style', 'display:none;');
                                            }

                                        }
                                    },
                                    Error: function (x, e) {
                                    }
                                });

    }


}

function sendData(dropid, divid) {

    var pid = document.getElementById("hdnProductID").value;
    var vid = document.getElementById(dropid).options[document.getElementById(dropid).selectedIndex].value;
    var valsel = dropid.replace('Selectvariant-', 'divselvalue-');
    if (document.getElementById(dropid).selectedIndex != 0) {
        if (document.getElementById(valsel) != null) {


            var valsel1 = eltSelect.id.replace('Selectvariant-', 'divcolspan-');
            document.getElementById(valsel1).innerHTML = "<span id=" + eltSelect.id.replace('Selectvariant-', 'spancolspan-') + ">" + document.getElementById(eltSelect.id.replace('Selectvariant-', 'spancolspan-')).innerHTML + "</span>" + document.getElementById(dropid).options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + document.getElementById(dropid).options[document.getElementById(dropid).selectedIndex].text.toString().toString().replace('($', ' - $').replace(')', '') + '</strong>';
        }
    }
    else {

        if (document.getElementById(valsel) != null) {
            document.getElementById(valsel).innerHTML = '';
        }
    }
    $.ajax(
                           {
                               type: "POST",
                               url: "/TestMail.aspx/Getvariantdata",
                               data: "{ProductId: " + pid + ",variantvalueId: " + vid + "}",
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               async: "true",
                               cache: "false",

                               success: function (msg) {

                                   if (document.getElementById(divid) != null) {
                                       $('#' + divid).html(msg.d);
                                       PriceChangeondropdown();

                                   }


                               },

                               Error: function (x, e) {

                                   PriceChangeondropdown();
                               }
                           });


}


function ChangeCustomprice() {

    if (document.getElementById('divcustomprice') != null) {
        $('#divcustomprice').attr('style', 'background: url(/images/priceloading.gif) no-repeat scroll 0 0 transparent;');
    }


    if (document.getElementById('ddlcustomwidth-ddl') != null) {

        document.getElementById('ddlcustomwidth-ddl').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text;
    }
    if (document.getElementById('ddlcustomlength-ddl') != null) {

        document.getElementById('ddlcustomlength-ddl').innerHTML = document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text;
    }
    if (document.getElementById('dlcustomqty-ddl') != null) {

        document.getElementById('dlcustomqty-ddl').innerHTML = document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].text;
    }
    if (document.getElementById('hdnpricetemp') != null && document.getElementById('hdnpricetemp').value == '') {
        if (document.getElementById('divcustomprice') != null) {
            document.getElementById('hdnpricetemp').value = document.getElementById('divcustomprice').innerHTML;
        }

    }
    var SalePriceTag;
    if (document.getElementById('ContentPlaceHolder1_hdnSalePriceTag') != null && document.getElementById('ContentPlaceHolder1_hdnSalePriceTag'.value) != '') {
        var SalePriceTag = ' <span style="float: left; background: none repeat scroll 0% 0% transparent;width:auto;">' + document.getElementById('ContentPlaceHolder1_hdnSalePriceTag').value + '</span>';
    }
    if (document.getElementById('divmeadetomeasure') != null) {

        $('#divmeadetomeasure').html('');

    }





    if (document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0) {


    }
    else {
        if (document.getElementById('divcolspancustom-1') != null) {

            document.getElementById('divcolspancustom-1').innerHTML = "<span id='spancolspancustom-1'>" + document.getElementById('spancolspancustom-1').innerHTML + "</span>Select " + document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex].text.toString().replace('($', ' - $').replace(')', '') + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);"  onclick="ShowModelForPleatGuide();" style="color:#B92127">LEARN MORE</a></div>';
        }
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex == 0) {


        document.getElementById('divcolspancustom-2').innerHTML = "<span id='spancolspancustom-2'>" + document.getElementById('spancolspancustom-2').innerHTML + "</span>Select Size:  " + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text.toLowerCase().replace('width', '') + '</strong> ' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text.toLowerCase().replace('length', '') + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);" onclick="variantDetail(\'divMakeOrderWidth\');" style="color:#B92127">LEARN MORE</a></div>';
    }
    else {
        if (document.getElementById('divcolspancustom-2') != null) {

            document.getElementById('divcolspancustom-2').innerHTML = "<span id='spancolspancustom-2'>" + document.getElementById('spancolspancustom-2').innerHTML + "</span>Select Size:  " + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text + '</strong> ' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text.toLowerCase().replace('length', '') + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);" onclick="variantDetail(\'divMakeOrderWidth\');" style="color:#B92127">LEARN MORE</a></div>';
        }
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex == 0) {


        document.getElementById('divcolspancustom-2').innerHTML = "<span id='spancolspancustom-2'>" + document.getElementById('spancolspancustom-2').innerHTML + "</span>Select Size:  " + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text.toLowerCase().replace('width', '') + '</strong> ' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text.toLowerCase().replace('length', '') + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);" onclick="variantDetail(\'divMakeOrderWidth\');" style="color:#B92127">LEARN MORE</a></div>';
    }
    else {

        document.getElementById('divcolspancustom-2').innerHTML = "<span id='spancolspancustom-2'>" + document.getElementById('spancolspancustom-2').innerHTML + "</span>Select Size:  " + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].text.toLowerCase().replace('width', '') + '</strong> ' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[0].text + ':<strong style="font-weight:normal;color:#B92127; margin-left:5px;">' + document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].text.toLowerCase().replace('length', '') + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);" onclick="variantDetail(\'divMakeOrderWidth\');" style="color:#B92127">LEARN MORE</a></div>';
    }
    if (document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null && document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex == 0) {


    }
    else {
        if (document.getElementById('divcolspancustom-3') != null && document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null) {

            document.getElementById('divcolspancustom-3').innerHTML = "<span id='spancolspancustom-3'>" + document.getElementById('spancolspancustom-3').innerHTML + "</span>Select " + document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex].text + '</strong><div style="float:right;line-height:25px;padding-right:2px;padding-top: 2px;"><a title="Learn More" href="javascript:void(0);" onclick="variantDetail(\'divMakeOrderOptions\');" style="color:#B92127">LEARN MORE</a></div>';
        }
    }
    if (document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex == 0) {


        document.getElementById('divcolspancustom-4').innerHTML = "<span id='spancolspancustom-4'>" + document.getElementById('spancolspancustom-4').innerHTML + "</span>" + document.getElementById('ContentPlaceHolder1_dlcustomqty').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>0</strong>";
    }
    else {
        if (document.getElementById('divcolspancustom-4') != null) {

            document.getElementById('divcolspancustom-4').innerHTML = "<span id='spancolspancustom-4'>" + document.getElementById('spancolspancustom-4').innerHTML + "</span>" + document.getElementById('ContentPlaceHolder1_dlcustomqty').options[0].text + ":<strong style='font-weight:normal;color:#B92127; margin-left:5px;'>" + document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].text + '</strong>';


        }
    }

    if (document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex != 0 && document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex != 0) {

        if (document.getElementById('ContentPlaceHolder1_btnAddTocartMade') != null) {
            document.getElementById('ContentPlaceHolder1_btnAddTocartMade').disabled = true;
        }

        if (document.getElementById('HdnPriceFb').value == "0") {
            document.getElementById('HdnPriceFb').value = document.getElementById("hdnProductID").value;
        }

        var pid = document.getElementById('HdnPriceFb').value;
        var sid = document.getElementById('ContentPlaceHolder1_ddlcustomstyle').options[document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex].value;
        var wid = document.getElementById('ContentPlaceHolder1_ddlcustomwidth').options[document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex].value;
        var lid = document.getElementById('ContentPlaceHolder1_ddlcustomlength').options[document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex].value;
        var oid = '';
        if (document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null) {
            oid = document.getElementById('ContentPlaceHolder1_ddlcustomoptin').options[document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex].value;
        }
        var qid = document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].value;
        var strMeasureValue = sid + ',' + wid + ',' + lid + ',' + oid + ',' + qid;
        var strMeasureName = 'Header,Width,Length,Options,Quantity (Panels)';
        $.ajax(
                           {
                               type: "POST",
                               url: "/TestMail.aspx/GetData",
                               data: "{ProductId: " + pid + ",Width: " + wid + ",Length: " + lid + ",Qty: " + qid + ",style: '" + sid + "',options: '" + oid + "' }",
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               async: "true",
                               cache: "false",

                               success: function (msg) {

                                   if (document.getElementById('divcustomprice') != null) {

                                       var msprie = parseFloat(msg.d);
                                       var qty = document.getElementById('ContentPlaceHolder1_dlcustomqty').options[document.getElementById('ContentPlaceHolder1_dlcustomqty').selectedIndex].value;
                                       msprie = parseFloat(msg.d) / parseFloat(qty);
                                       $('#divcustomprice').html('<label style="float: left;">$' + msprie.toFixed(2) + '</label>' + SalePriceTag);
                                       $('#divtotalcustomprice').html('$' + msg.d);
                                       document.getElementById('ContentPlaceHolder1_hdnpricecustomcart').value = msg.d;
                                   }
                                   if (document.getElementById('ContentPlaceHolder1_btnAddTocartMade') != null) {
                                       document.getElementById('ContentPlaceHolder1_btnAddTocartMade').disabled = false;
                                   }
                               },
                               Error: function (x, e) {
                               }
                           });
    }
    else {
        if (document.getElementById('divcustomprice') != null) {
            $('#divcustomprice').html(document.getElementById('hdnpricetemp').value);
        }
    }
    if (document.getElementById('divcustomprice') != null) {
        $('#divcustomprice').attr('style', 'background: none;');
    }

    $.ajax(
                {
                    type: "POST",
                    url: "/TestMail.aspx/GetDataAdminMessage",
                    data: "{ProductId: " + pid + ",ProductType: 2,Qty: " + qid + ",vValueid: '" + strMeasureValue + "',vNameid: '" + strMeasureName + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (msg) {


                        if (document.getElementById('divmeadetomeasure') != null) {

                            $('#divmeadetomeasure').html('<label style="width: 100% !important;color:#B92127" class="readymade-detail-left">' + msg.d + '</label>');
                            $('#divmeadetomeasure').attr('style', 'display:block;');

                        }
                        //}
                    },
                    Error: function (x, e) {
                    }
                });

}

function chkHeight() {

    if (document.getElementById('prepage')) {
        var windowHeight = 0;
        windowHeight = $(document).height(); //window.innerHeight;

        document.getElementById('prepage').style.height = windowHeight + 'px';
        document.getElementById('prepage').style.display = '';
    }
}

var $j = jQuery.noConflict();
$j(document).ready(function () {


    if (document.getElementById('ContentPlaceHolder1_hdnIsShowImageZoomer').value == "true" && document.getElementById('ContentPlaceHolder1_imgMain').src.indexOf('image_not_available') <= -1) {
        var imagename = '';
        if (document.getElementById('ContentPlaceHolder1_imgMain')) {
            imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
        }
        imagename = imagename.replace('medium', 'large');


        $j('#ContentPlaceHolder1_imgMain').addimagezoom({
            zoomrange: [3, 10],
            magnifiersize: [300, 300],
            magnifierpos: 'right',
            cursorshade: true,
            largeimage: imagename //<-- No comma after last option!
        });
        $j("#Button2").click(function () {
            var imagename = document.getElementById('ContentPlaceHolder1_imgMain').src;
            imagename = imagename.toLowerCase().replace('medium', 'large');

            $j('#ContentPlaceHolder1_imgMain').addimagezoom({
                zoomrange: [3, 10],
                magnifiersize: [300, 300],
                magnifierpos: 'right',
                cursorshade: true,
                largeimage: imagename + '?54666'
            })
        });

    }

});


function selecteddropdownvalue(dropid, valid) {
    var ddlname = 'Selectvariant-' + dropid;
    var ddl = 'divSelectvariant-' + dropid;
    var allselect = document.getElementById(ddl).getElementsByTagName('div');
    for (var iS = 0; iS < allselect.length; iS++) {
        var eltSelect = allselect[iS];
        if (eltSelect.id.toString().indexOf("divflat-radio-" + dropid + "-" + valid + "") > -1) {
            eltSelect.className = '';
            eltSelect.className = 'iradio_flat-red checked';

        }
        else if (eltSelect.id.toString().indexOf("divflat-radio-" + dropid + "-") > -1) {
            eltSelect.className = '';
            eltSelect.className = 'iradio_flat-red';

        }
    }

    document.getElementById(ddlname).value = valid;
}
function selecteddropdownvaluecustom(dropid, valid, divid, randeid) {

    var allselect = document.getElementById('ContentPlaceHolder1_divcustomfabric').getElementsByTagName('div');
    for (var iS = 0; iS < allselect.length; iS++) {
        var eltSelect = allselect[iS];

        if (eltSelect.id.toString().toLowerCase().indexOf('divcustomflat-radio-' + randeid) > -1) {

            if (eltSelect.id.toString().toLowerCase() == divid.toLowerCase()) {
                eltSelect.className = '';
                eltSelect.className = 'iradio_flat-red checked';
                //eltSelect.style.zIndex = '-1';
            }
            else if (eltSelect.id.toString().indexOf("divcustomflat-radio-") > -1) {
                eltSelect.className = '';
                eltSelect.className = 'iradio_flat-red';

            }
        }
    }

    document.getElementById(dropid).value = valid;
}
function ShowModelHelp(id) {


    //document.getElementById('frmdisplay').height = '720px';
    //document.getElementById('frmdisplay').width = '620px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:620px;height:720px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '620px';
    //document.getElementById('popupContact').style.height = '720px';
    //window.scrollTo(0, 0);

    //document.getElementById('btnreadmore').click();
    //var imgnm = document.getElementById('ContentPlaceHolder1_imgMain').src;
    //document.getElementById('frmdisplay').src = imgnm;//'/MoreImages.aspx?PID=' + id + '&img=' + imgnm;

    if (document.getElementById("ContentPlaceHolder1_imgMain") != null) {

        document.getElementById("ContentPlaceHolder1_imgMain").click();
    }

}
function ShowInventoryMessage(result) {


    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    //document.getElementById('frmdisplay').height = '130px';
    //document.getElementById('frmdisplay').width = '565px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:130px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '565px';
    //document.getElementById('popupContact').style.height = '130px';
    //window.scrollTo(0, 0);


    //var root = window.location.protocol + '//' + window.location.host;
    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + result;
    //centerPopup();

    //loadPopup();

    document.getElementById("hdnhtmlall").value = result;
    //$('#ainventorymsg').attr('href', '/inventorymessage.aspx');
    document.getElementById('ainventorymsg').click();
}
function ShowSwatchMessage() {

    //document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '';
    //document.getElementById('frmdisplay').height = '225px';
    //document.getElementById('frmdisplay').width = '565px';
    //document.getElementById('popupContact').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:565px;height:225px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");

    //document.getElementById('popupContact').style.width = '565px';
    //document.getElementById('popupContact').style.height = '225px';
    //window.scrollTo(0, 0);


    //$.ajax(
    //            {
    //                type: "POST",
    //                url: "/TestMail.aspx/GeLimitMessage",
    //                data: "{PId: 1}",
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                async: "true",
    //                cache: "false",
    //                success: function (msg) {
    //                    if (document.getElementById('divswatchinfo') != null) {
    //                        var root = window.location.protocol + '//' + window.location.host;

    //                        document.getElementById("frmdisplay").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css?6566" rel="stylesheet" type="text/css" />' + msg.d;

    //                    }
    //                },
    //                Error: function (x, e) {
    //                }
    //            });

    //centerPopup();
    //loadPopup();
    document.getElementById('afreeswatchmsg').click();


}

function SelectVariantBypostback(strname, strvalue) {
    var variantname = strname.split(",");
    var variantvalue = strvalue.split(",");

    for (i = 0; i < variantname.length; i++) {
        if (variantvalue[i].toLowerCase().indexOf('select ') > -1) {
            if (document.getElementById(variantname[i]) != null) {
                document.getElementById(variantname[i]).value = '0';
            }
        }
        else {
            if (document.getElementById(variantname[i]) != null) {
                document.getElementById(variantname[i]).value = variantvalue[i];
            }
        }


    }

}

function variantDetail(divid) {
    centerPopup1();

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '500px';
    document.getElementById('frmdisplay1').width = '517px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:517px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '517px';
    document.getElementById('popupContact1').style.height = '500px';

    document.getElementById('btnhelpdescri').click();
    if (divid == 'divMakeOrderWidth') {
        document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = document.getElementById(divid).innerHTML + '<br />' + document.getElementById('divMakeOrderLength').innerHTML;
    }
    else {
        document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = document.getElementById(divid).innerHTML;
    }
}
function variantDetailreturnpolicy(divid) {
    disablePopup();
    centerPopup1();

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '500px';
    document.getElementById('frmdisplay1').width = '810px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:810px;height:500px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '810px';
    document.getElementById('popupContact1').style.height = '500px';

    document.getElementById('btnhelpdescri').click();

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = document.getElementById(divid).innerHTML;
}
function ShowModelHelpShipping(id) {

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '620px';
    document.getElementById('frmdisplay1').width = '830px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:830px;height:843px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '830px';
    document.getElementById('popupContact1').style.height = '620px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}

function ShowModelForNotification(id) {

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '400px';
    document.getElementById('frmdisplay1').width = '610px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:610px;height:400px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '610px';
    document.getElementById('popupContact1').style.height = '400px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}

function ShowModelForInappropriateRating(id) {

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '400px';
    document.getElementById('frmdisplay1').width = '610px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:610px;height:400px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '610px';
    document.getElementById('popupContact1').style.height = '400px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}

function ShowModelForPriceMatch(id) {

    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '650px';
    document.getElementById('frmdisplay1').width = '600px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:600px;height:650px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '600px';
    document.getElementById('popupContact1').style.height = '650px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    document.getElementById('frmdisplay1').src = id;
}
function ShowModelForMeasuringguide() {

    centerPopup1();
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '600px';
    document.getElementById('frmdisplay1').width = '970px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '970px';
    document.getElementById('popupContact1').style.height = '600px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divmeasuring').innerHTML;

}
function ShowModelFordesignguide() {

    centerPopup1();
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '500px';
    document.getElementById('frmdisplay1').width = '970px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '970px';
    document.getElementById('popupContact1').style.height = '500px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divPleatGuide').innerHTML + document.getElementById('divMakeOrderWidth').innerHTML.toString().replace('id="info-div-main"', 'id="info-div-main" style="width:97% !important;"').replace('class="static_content-main"', 'class="static_content-main" style="width:97% !important;"').replace(/<p style="/g, '<p style="width:97% !important; ').replace(/width: 474px;/g, '') + document.getElementById('divMakeOrderLength').innerHTML.toString().replace('id="info-div-main"', 'id="info-div-main" style="width:97% !important;"').replace('class="static_content-main"', 'class="static_content-main" style="width:97% !important;"').replace(/<p style="/g, '<p style="width:97% !important; ').replace(/width: 474px;/g, '') + document.getElementById('divMakeOrderOptions').innerHTML.toString().replace('id="info-div-main"', 'id="info-div-main" style="width:97% !important;"').replace('class="static_content-main"', 'class="static_content-main" style="width:97% !important;"').replace(/<p style="/g, '<p style="width:97% !important; ').replace(/width: 474px;/g, '');

}
function Showretrunnguide() {

    centerPopup1();
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '500px';
    document.getElementById('frmdisplay1').width = '970px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '970px';
    document.getElementById('popupContact1').style.height = '500px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divreturnpolicy').innerHTML;

}

function ShowModelForfaq() {
    centerPopup1();
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '600px';
    document.getElementById('frmdisplay1').width = '970px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '970px';
    document.getElementById('popupContact1').style.height = '600px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" /><div class=\"static-title\"><span style=\"padding-left: 10px;\">FAQ</span></div>' + document.getElementById('divfaq').innerHTML;
}

function ShowModelForPleatGuide() {

    centerPopup1();
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
    document.getElementById('frmdisplay1').height = '600px';
    document.getElementById('frmdisplay1').width = '970px';
    document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:970px;height:600px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
    document.getElementById('popupContact1').style.width = '970px';
    document.getElementById('popupContact1').style.height = '600px';
    window.scrollTo(0, 0);
    document.getElementById('btnhelpdescri').click();
    var root = window.location.protocol + '//' + window.location.host;
    document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '<link href="' + root + '/css/style.css" rel="stylesheet" type="text/css" />' + document.getElementById('divPleatGuide').innerHTML;
}
function clearcomment() {
    if (document.getElementById('ContentPlaceHolder1_txtname') != null) { document.getElementById('ContentPlaceHolder1_txtname').value = 'Enter your name'; }
    if (document.getElementById('ContentPlaceHolder1_txtEmail') != null) { document.getElementById('ContentPlaceHolder1_txtEmail').value = 'Enter your email address'; }
    if (document.getElementById('ContentPlaceHolder1_txtcomment') != null) { document.getElementById('ContentPlaceHolder1_txtcomment').value = 'Enter your comment'; }
    if (document.getElementById('ContentPlaceHolder1_ddlrating') != null) {
        document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex = 0;
        ratingImage();
    }
}

function onKeyPressBlockNumbers1(e) {
    var key = window.event ? window.event.keyCode : e.which;
    if (key != 46) {
        if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
            return key;
        }
    }
    if (key == 13) {
        if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
            document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
        }
    }
    var keychar = String.fromCharCode(key);
    var reg = /\d/;
    if (window.event)
        return event.returnValue = reg.test(keychar);
    else
        return reg.test(keychar);
}
function onKeyPressBlockNumbersOnly(e) {
    var key = window.event ? window.event.keyCode : e.which;

    if (key != 46) {
        if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
            return key;
        }
    }
    if (key == 13) {
        if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
            document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
        }
    }
    var keychar = String.fromCharCode(key);
    var reg = /\d/;
    if (window.event)
        return event.returnValue = reg.test(keychar);
    else
        return reg.test(keychar);
}


function checkquantity() {


    if ((document.getElementById("ContentPlaceHolder1_txtQty").value == '') || (document.getElementById("ContentPlaceHolder1_txtQty").value <= 0) || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value)) {
        jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty");
        document.getElementById("ContentPlaceHolder1_txtQty").value = 1; return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_divOptionalAccessories")) {
        var allElts = document.getElementById("ContentPlaceHolder1_gvOptionalAcc").getElementsByTagName('INPUT');
        var i;
        for (i = 0; i < allElts.length; i++) {
            var elt = allElts[i];
            if (elt.type == "checkbox" && elt.checked == true) {
                var refQty = elt.id.replace('ckhOASelect', 'txtOAQty');

                if (document.getElementById(refQty) != null && (document.getElementById(refQty).value == '' || document.getElementById(refQty).value == '0')) {
                    jAlert("Please enter valid Quantity.", "Message", refQty);
                    return false;
                }
            }
        }
    }


    else { document.getElementById("prepage").style.display = ''; return true; }
}





function removeelement(id) {
    document.getElementById(id).innerHTML = '';
}

function AddElement() {
}

function SetTimee() {

    setInterval("CheckTimer();", 1000);
    tabberAutomatic();
}

function moneyConvert(value) {
    var buf = "";
    var sBuf = "";
    var j = 0;
    value = String(value);
    if (value.indexOf(".") > 0) {
        buf = value.substring(0, value.indexOf("."));
    } else {
        buf = value;
    }
    if (buf.length % 3 != 0 && (buf.length / 3 - 1) > 0) {
        sBuf = buf.substring(0, buf.length % 3) + ",";
        buf = buf.substring(buf.length % 3);
    }
    j = buf.length;
    for (var i = 0; i < (j / 3 - 1) ; i++) {
        sBuf = sBuf + buf.substring(0, 3) + ",";
        buf = buf.substring(3);
    }
    sBuf = sBuf + buf;
    if (value.indexOf(".") > 0) {
        value = sBuf + value.substring(value.indexOf("."));
    }
    else {
        value = sBuf;
    }
    return value;
}


function checkDate(sender, args) {
}
function ShowImagevideo() {

    if (document.getElementById('videoid') != null && document.getElementById('videoid').src.indexOf('play') > -1) {

        document.getElementById('Hidden1').value = '0';

        if (document.getElementById('imgMain') != null) {

            document.getElementById('Button1').click();

        }
        document.getElementById('ContentPlaceHolder1_imgMain').style.display = 'none';
        if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
            document.getElementById('ContentPlaceHolder1_divzioom').style.display = 'none';

        }
        document.getElementById('divvedeo').style.display = '';
        document.getElementById('videoid').src = '/images/pause.png';
    }
    else {
        document.getElementById('Hidden1').value = '1';
        document.getElementById('ContentPlaceHolder1_imgMain').style.display = '';
        if (document.getElementById('ContentPlaceHolder1_divzioom') != null) {
            document.getElementById('ContentPlaceHolder1_divzioom').style.display = '';
        }
        document.getElementById('divvedeo').style.display = 'none';
        document.getElementById('videoid').src = '/images/play.png';
        if (document.getElementById('imgMain') != null) {
            document.getElementById('Button1').click();
        }
    }
}

function ratingImage() {
    var indx = document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex;
    if (indx == 0) {
        document.getElementById("img1").src = '/images/star-form1.jpg';
        document.getElementById("img2").src = '/images/star-form1.jpg';
        document.getElementById("img3").src = '/images/star-form1.jpg';
        document.getElementById("img4").src = '/images/star-form1.jpg';
        document.getElementById("img5").src = '/images/star-form1.jpg';
    }
    else if (indx == 1) {
        document.getElementById("img1").src = '/images/star-form.jpg';
        document.getElementById("img2").src = '/images/star-form1.jpg';
        document.getElementById("img3").src = '/images/star-form1.jpg';
        document.getElementById("img4").src = '/images/star-form1.jpg';
        document.getElementById("img5").src = '/images/star-form1.jpg';

    }
    else if (indx == 2) {
        document.getElementById("img1").src = '/images/star-form.jpg';
        document.getElementById("img2").src = '/images/star-form.jpg';
        document.getElementById("img3").src = '/images/star-form1.jpg';
        document.getElementById("img4").src = '/images/star-form1.jpg';
        document.getElementById("img5").src = '/images/star-form1.jpg';

    }
    else if (indx == 3) {
        document.getElementById("img1").src = '/images/star-form.jpg';
        document.getElementById("img2").src = '/images/star-form.jpg';
        document.getElementById("img3").src = '/images/star-form.jpg';
        document.getElementById("img4").src = '/images/star-form1.jpg';
        document.getElementById("img5").src = '/images/star-form1.jpg';

    }
    else if (indx == 4) {
        document.getElementById("img1").src = '/images/star-form.jpg';
        document.getElementById("img2").src = '/images/star-form.jpg';
        document.getElementById("img3").src = '/images/star-form.jpg';
        document.getElementById("img4").src = '/images/star-form.jpg';
        document.getElementById("img5").src = '/images/star-form1.jpg';

    }
    else if (indx == 5) {
        document.getElementById("img1").src = '/images/star-form.jpg';
        document.getElementById("img2").src = '/images/star-form.jpg';
        document.getElementById("img3").src = '/images/star-form.jpg';
        document.getElementById("img4").src = '/images/star-form.jpg';
        document.getElementById("img5").src = '/images/star-form.jpg';

    }
}


function CheckExits() {

    if (document.getElementById('ContentPlaceHolder1_txtname').value.replace(/^\s+|\s+$/g, "") == '') {
        jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');

        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtname').value.toString().toLowerCase() == 'enter your name') {
        jAlert('Please Enter Name.', 'Message', 'ContentPlaceHolder1_txtname');

        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") == '') {
        jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');

        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtEmail').value.toString().toLowerCase() == 'enter your email address') {
        jAlert('Please Enter Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');

        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, ""))) {
        jAlert('Please Enter Valid Email Address.', 'Message', 'ContentPlaceHolder1_txtEmail');

        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtcomment').value.replace(/^\s+|\s+$/g, "") == '') {
        jAlert('Please Enter Comment', 'Message', 'ContentPlaceHolder1_txtcomment');

        return false;
    }
    if (document.getElementById('ContentPlaceHolder1_txtcomment').value.toString().toLowerCase() == 'enter your comment') {
        jAlert('Please Enter Comment.', 'Message', 'ContentPlaceHolder1_txtcomment');

        return false;
    }

    if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {
        jAlert('Please Select Rating.', 'Message', 'ContentPlaceHolder1_ddlrating');

        return false;
    }
    document.getElementById('prepage').style.display = '';
    if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {
        $find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._stopTimer();
    }
    return true;
}
var Emailresults
function checkemail1(str) {
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
    if (filter.test(str))
        Emailresults = true
    else {
        Emailresults = false
    }
    return (Emailresults)
}