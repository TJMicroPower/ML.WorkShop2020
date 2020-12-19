function cleanwordformat() {
    var text = '';
    var oEditor;
    if (arguments.length == 0) {
        oEditor = FCKeditorAPI.GetInstance("ArticleContent")
        text = oEditor.GetXHTML();
    } else {
    oEditor = CKEDITOR.instances[arguments[0]];
    text = oEditor.getData();
    }

    text = text.replace(/<FONT[^>]*>/gi, "");
    text = text.replace(/<\/FONT>/gi, "");
    text = text.replace(/<U>/gi, "");
    text = text.replace(/<\/U>/gi, "");
    text = text.replace(/<H[^>]*>/gi, "");
    text = text.replace(/<\/H[^>]*>/gi, "");

    //  Change  these  tags.
    //text=text.replace(/<B[^>]*>/gi,"&bold");
    //text=text.replace(/<\/B[^>]*>/gi,"&cbold");
    text = text.replace(/<B>/gi, "&bold");
    text = text.replace(/<\/B>/gi, "&cbold");
    text = text.replace(/<STRONG[^>]*>/gi, "&bold");
    text = text.replace(/<\/STRONG[^>]*>/gi, "&cbold");

    text = text.replace(/<I>/gi, "&ital");
    text = text.replace(/<\/I[^>]*>/gi, "&cital");
    text = text.replace(/<EM[^>]*>/gi, "&ital");
    text = text.replace(/<\/EM[^>]*>/gi, "&cital");

    text = text.replace(/<UL[^>]*>/gi, "&ultag");
    text = text.replace(/<LI[^>]*>/gi, "&litag");
    text = text.replace(/<OL[^>]*>/gi, "&oltag");
    text = text.replace(/<\/OL>/gi, "&olctag");
    text = text.replace(/<\/LI>/gi, "&lictag");
    text = text.replace(/<\/UL>/gi, "&ulctag");

    text = text.replace(/<P[^>]*align=center*>/gi, "<p align=center>");
    //text=text.replace(/<P[^>]*>/gi,"&parag");
    text = text.replace(/<\/P>/gi, "");

    /*
    text=text.replace(/?gi,'\"');
    text=text.replace(/?gi,'\"');
    text=text.replace(/?gi,'\"');
    text=text.replace(/mailto:/gi,'\"');
    text=text.replace(/?g,"&Auml;");
    text=text.replace(/?g,"&Ouml;");
    text=text.replace(/?g,"&Uuml;");
    text=text.replace(/?g,"&auml;");
    text=text.replace(/?g,"&ouml;");
    text=text.replace(/?g,"&uuml;");
    text=text.replace(/?gi,"&szlig;"); 
    */

    text = text.replace(/&lt;[^>]&gt*;/gi, "");
    text = text.replace(/&lt;\/[^>]&gt*;/gi, "  ");
    text = text.replace(/<o:[^>]*>/gi, "");
    text = text.replace(/<\/o:[^>]*>/gi, "");
    text = text.replace(/<\?xml:[^>]*>/gi, "");
    text = text.replace(/<\/?st[^>]*>/gi, "");
    text = text.replace(/<[^>]*</gi, "<");
    text = text.replace(/<SPAN[^>]*>/gi, "");
    text = text.replace(/<SPAN[^class]*>/gi, "");
    //text=text.replace(/<SPAN[^style]*>/gi,"");
    text = text.replace(/<\/SPAN>/gi, "");
    //text=text.replace(/<\/A>/gi,"");

    //  Clear  the  inner  parts  of  other  tags.
    text = text.replace(/style=[^>]*"/g, '  ');
    text = text.replace(/style=[^>]*'/g, "  ");
    text = text.replace(/style=[^>]*>/g, ">");
    text = text.replace(/lang=[^>]*>/g, ">");
    text = text.replace(/name=[^>]*  /g, "");
    text = text.replace(/name=[^>]*>/g, ">");
    text = text.replace(/<A[^>]*>/g, "");

    //text=text.replace(/<p[^>]*>/gi,"<p>");


    //  Put  the  tags  back
    text = text.replace(/&bold/g, "<B>");
    text = text.replace(/&cbold/g, "</B>");

    text = text.replace(/&ital/g, "<EM>");
    text = text.replace(/&cital/g, "</EM>");

    text = text.replace(/&ultag/g, "<UL>");
    text = text.replace(/&litag/g, "<LI>");
    text = text.replace(/&oltag/g, "<OL>");
    text = text.replace(/&olctag/g, "<\/OL>");
    text = text.replace(/&lictag/g, "<\/LI>");
    text = text.replace(/&ulctag/g, "<\/UL>");

    text = text.replace(/vAlign=bottom/g, "");
    text = text.replace(/noWrap/g, "");
    text = text.replace(/border=0/g, "border=1");
    text = text.replace(/class=MsoNormalTable/g, "");
    text = text.replace(/<B><\/B>/gi, '');
    text = text.replace(/<DIV><\/DIV>/gi, '');

    if (arguments.length == 0)
        oEditor.SetHTML(text);
    else
        oEditor.setData(text);
};