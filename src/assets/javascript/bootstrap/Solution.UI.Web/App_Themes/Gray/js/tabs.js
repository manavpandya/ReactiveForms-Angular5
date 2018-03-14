/***************************/
//@Author: Adrian "yEnS" Mato Gondelle & Ivan Guardado Castro
//@website: www.yensdesign.com
//@email: yensamg@gmail.com
//@license: Feel free to use it, but keep this credits please!					
/***************************/

$(document).ready(function () {
    $(".menu > li").click(function (e){
        switch (e.target.id) {

            case "OrdersList":
                //change status & style menu
                $("#OrdersList").addClass("active");
                $("#AmountList").removeClass("active");

                //display selected division, hide others
                $("div.orderchart").fadeIn();
                $("div.amountchart").css("display", "none");

                break;

            case "AmountList":
                //change status & style menu
                $("#OrdersList").removeClass("active");
                $("#AmountList").addClass("active");

                //display selected division, hide others
                $("div.amountchart").fadeIn();
                $("div.orderchart").css("display", "none");

                break;
            case "ordernotes":
                //change status & style menu
                $("#ordernotes").addClass("active");
                $("#privatenotes").removeClass("active");
                $("#giftnotes").removeClass("active");
                $("#myaccount").removeClass("active");
                $("#yearly").removeClass("active");
                //display selected division, hide others
                $("div.order-notes").fadeIn();
                $("div.private-notes").css("display", "none");
                $("div.gift-notes").css("display", "none");
                $("div.myaccount").css("display", "none");
                  $("div.yearly").css("display", "none");
                break;
            case "privatenotes":
                //change status & style menu
                $("#ordernotes").removeClass("active");
                $("#privatenotes").addClass("active");
                $("#giftnotes").removeClass("active");
                $("#myaccount").removeClass("active");
                 $("#yearly").removeClass("active");
                //display selected division, hide others
                $("div.private-notes").fadeIn();
                $("div.order-notes").css("display", "none");
                $("div.gift-notes").css("display", "none");
                $("div.my-account").css("display", "none");
                 $("div.yearly").css("display", "none");
                break;

          
            case "giftnotes":
                //change status & style menu
                $("#ordernotes").removeClass("active");
                $("#privatenotes").removeClass("active");
                $("#giftnotes").addClass("active");
             
                $("#myaccount").removeClass("active");
                 $("#yearly").removeClass("active");
                //display selected division, hide others
                $("div.gift-notes").fadeIn();
                $("div.order-notes").css("display", "none");
                $("div.private-notes").css("display", "none");
                $("div.my-account").css("display", "none");
                $("div.yearly").css("display", "none");
                break;
            case "myaccount":
                //change status & style menu
                $("#ordernotes").removeClass("active");
                $("#privatenotes").removeClass("active");
                $("#giftnotes").removeClass("active");
                
                $("#myaccount").addClass("active");
                  $("#yearly").removeClass("active");
                //display selected division, hide others
                $("div.my-account").fadeIn();
                $("div.order-notes").css("display", "none");
                $("div.private-notes").css("display", "none");
                $("div.gift-notes").css("display", "none");
                 $("div.yearly").css("display", "none");
                break;
                        case "yearly": 
                            //change status & style menu 
                            $("#ordernotes").removeClass("active"); 
                            $("#privatenotes").removeClass("active"); 
                            $("#giftnotes").removeClass("active"); 
                            $("#myaccount").removeClass("active"); 
                            $("#yearly").addClass("active"); 
                            //display selected division, hide others 
                            $("div.yearly").fadeIn(); 
                            $("div.order-notes").css("display", "none"); 
                            $("div.private-notes").css("display", "none"); 
                            $("div.gift-notes").css("display", "none");
                            $("div.my-account").css("display", "none");
                            //$("div.yearly").css("class", "active"); 
                            break; 
        }
        //alert(e.target.id);
        
    });
});
