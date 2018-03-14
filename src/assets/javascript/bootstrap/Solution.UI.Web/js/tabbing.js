// JavaScript Document
		
		function tabon_outfit_quick_links(myClass, val){

		//activate clicked tab
		navRoot = document.getElementById("outfit_quick_links");
		var num = 0;
		for (i=0; i<navRoot.childNodes.length; i++) {
			node = navRoot.childNodes[i];
				if (node.nodeName=="A") {
					num++;
					node.className=node.className.replace(" over", "");
					if(myClass == node.className){
						node.className+=" over";
					}
				}
		}
		//show hide particular data on tab click
		for(i=1;i<=num;i++)
			{
				if(("div"+i)!=val){
					document.getElementById("div"+i).style.display='none';
				}
			}
		if(document.getElementById(val).style.display=='none')
			{
				document.getElementById(val).style.display='';
			}
			/*
			else
			{
				document.getElementById(val).style.display='none';
			}
			*/
}


function tabon_quick_links(myClass, val){

		//activate clicked tab
		navRoot = document.getElementById("quick_links");
		
		var num = 0;
		for (i=0; i<navRoot.childNodes.length; i++) {
			node = navRoot.childNodes[i];
				if (node.nodeName=="A") {
					num++;
					node.className=node.className.replace(" over", "");
					if(myClass == node.className){
						node.className+=" over";
					}
				}
		}
		
		//show hide particular data on tab click
		for(i=1;i<=num;i++)
			{
				
				if(("quick_links"+i)!=val){
					document.getElementById("quick_links"+i).style.display='none';
				}
			}
		if(document.getElementById(val).style.display=='none')
			{
				document.getElementById(val).style.display='';
			}
			/*
			else
			{
				document.getElementById(val).style.display='none';
			}
			*/
}

// JavaScript Document
		
		function tabon_outfit_quick_links_right(myClass, val){

		//activate clicked tab
		navRoot = document.getElementById("outfit_quick_links_right");
		var num = 0;
		for (i=0; i<navRoot.childNodes.length; i++) {
			node = navRoot.childNodes[i];
				if (node.nodeName=="A") {
					num++;
					node.className=node.className.replace(" over", "");
					if(myClass == node.className){
						node.className+=" over";
					}
				}
		}
		//show hide particular data on tab click
		for(i=1;i<=num;i++)
			{
				if(("divv"+i)!=val){
					document.getElementById("divv"+i).style.display='none';
				}
			}
		if(document.getElementById(val).style.display=='none')
			{
				document.getElementById(val).style.display='';
			}
			/*
			else
			{
				document.getElementById(val).style.display='none';
			}
			*/
}


function tabon_quick_links_right(myClass, val){

		//activate clicked tab
		navRoot = document.getElementById("quick_links_right");
		
		var num = 0;
		for (i=0; i<navRoot.childNodes.length; i++) {
			node = navRoot.childNodes[i];
				if (node.nodeName=="A") {
					num++;
					node.className=node.className.replace(" over", "");
					if(myClass == node.className){
						node.className+=" over";
					}
				}
		}
		
		//show hide particular data on tab click
		for(i=1;i<=num;i++)
			{
				
				if(("quick_links_right"+i)!=val){
					document.getElementById("quick_links_right"+i).style.display='none';
				}
			}
		if(document.getElementById(val).style.display=='none')
			{
				document.getElementById(val).style.display='';
			}
			/*
			else
			{
				document.getElementById(val).style.display='none';
			}
			*/
}
