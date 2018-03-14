$(document).ready(function() {
  $('.catNav ul li').hover(function() {
    $(this).addClass('selected2');
  }, function() {
    $(this).removeClass('selected2');
  });
});


function changeTabbedItem(b){
if(b == "1")
{
$(".tabbedLinks").css("background", "url(http://lib.store.yahoo.net/lib/yhst-128709672515041/ey-tabbedLinksBG.jpg) no-repeat scroll center top");
}
else if(b == "2")
{
$(".tabbedLinks").css("background", "url(http://lib.store.yahoo.net/lib/yhst-128709672515041/ey-tabbedLinksBG.jpg) no-repeat scroll center -39px");
}
else if(b == "3")
{
$(".tabbedLinks").css("background", "url(http://lib.store.yahoo.net/lib/yhst-128709672515041/ey-tabbedLinksBG.jpg) no-repeat scroll center -78px");
}
else if(b == "4")
{
$(".tabbedLinks").css("background", "url(http://lib.store.yahoo.net/lib/yhst-128709672515041/ey-tabbedLinksBG.jpg) no-repeat scroll center -117px");
}
else
{
$(".tabbedLinks").css("background", "url(http://lib.store.yahoo.net/lib/yhst-128709672515041/ey-tabbedLinksBG.jpg) no-repeat scroll center -156px");
}

$(".tabbedA").removeClass('selectedTab');
$("#TabbedA" + b).addClass('selectedTab');
$(".tabbedItemsTable").css("display", "none");
$("#TabbedItemsTable" + b).css("display", "block");

}


function changeTab(b){
$(".tabs").css("display","none");
$("#Tab"+b).css("display","block");
}

function changeTopNav(b)
{
if(b == "1")
{
$(".topNavA1").css("background", "url(https://lib.store.yahoo.net/lib/yhst-128709672515041/ey-topNav3.jpg) no-repeat scroll left -30px");
}
else if(b == "2")
{
$(".topNavA2").css("background", "url(https://lib.store.yahoo.net/lib/yhst-128709672515041/ey-topNav3.jpg) no-repeat scroll -193px -30px");
}
else if(b == "3")
{
$(".topNavA3").css("background", "url(https://lib.store.yahoo.net/lib/yhst-128709672515041/ey-topNav3.jpg) no-repeat scroll -378px -30px");
}
}