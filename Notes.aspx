<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Notes.aspx.vb" ClientIDMode="Static" Inherits="NotesNew" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
  <title>ISGEC-Notes</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <script src="App_Services/jquery-3.3.1.min.js"></script>
  <link href="Res/fa/css/all.css" rel="stylesheet" />
  <style>
    html, body {
      background-color: black;
      font-family: 'Courier New';
      font-size: 16px;
    }
    p{
      margin:2px;
      font-size:14px;
      font-weight:bold;
    }

    .nt-conainer {
      position:absolute;
      top:0px;
      right:0px;
      bottom:0px;
      left:0px;
      display: flex;
      flex-direction: column;
      margin: 3px !important;
      border-radius: 6px;
      background-color: white;
    }

    .nt-header {
      display: flex;
      flex-direction: row;
      justify-content: space-between;
      background-color: darkcyan;
      border-top-left-radius: inherit;
      border-top-right-radius: inherit;
      padding: 2px;
      color: white;
    }

    .nt-body {
      height: 100%;
      padding: 2px;
      overflow-y: scroll;
      overflow-x: hidden;
      scrollbar-base-color: red;
      border-bottom-left-radius: inherit;
      border-bottom-right-radius: inherit;
    }

    .nt-input-box {
      width: 100%;
      border: 1pt solid gray;
      border-radius: 4px;
      font-family: 'Courier New';
      font-size: 12px;
    }

    .nt-but-danger {
      border-radius: 4px;
      border: 1pt solid #ff0000;
      background-color: crimson;
      color: white;
      font-family: 'Courier New';
      font-size: 14px;
    }

      .nt-but-danger:hover {
        border-radius: 4px;
        border: 1pt solid #ff0000;
        background-color: #fa7d7d;
        color: white;
      }
    .nt-but-primary {
      border-radius: 4px;
      border: 1pt solid #1f336d;
      background-color:#2f5fe9;
      color: white;
      font-family: 'Courier New';
      font-size: 14px;
    }

      .nt-but-primary:hover {
        border-radius: 4px;
        border: 1pt solid #1f336d;
        background-color: #698bed;
        color: white;
      }
    .nt-but-grey {
      border-radius: 4px;
      border: 1pt solid #b7b5b5;
      background-color:#d7d5d5;
      color:black;
      font-family: 'Courier New';
      font-size: 14px;
    }

      .nt-but-grey:hover {
        border-radius: 4px;
        border: 1pt solid #b7b5b5;
        background-color:#f2f2f2;
        color:red;
      }
    .nt-but-readmore {
      border-radius: 4px;
      border: 1pt solid #b7b5b5;
      background-color:#d7d5d5;
      color:black;
      font-family:Tahoma;
      font-weight:bold;
      font-size: 8px;
    }

      .nt-but-readmore:hover {
        border-radius: 4px;
        border: 1pt solid #b7b5b5;
        background-color:#f2f2f2;
        color:red;
      }

    .nt-but-success {
      border-radius: 4px;
      border: 1pt solid #049317;
      background-color: #06bf1e;
      color: white;
      font-family: 'Courier New';
      font-size: 14px;
    }

      .nt-but-success:hover {
        border-radius: 4px;
        background-color: #05fa25;
        color: black;
      }

    .nt-modal-container{
      display: none; 
      position: fixed;
      z-index: 1; 
      left: 0;
      top: 0;
      width: 100%;
      height: 100%;
      overflow:hidden;
      background-color: rgb(0,0,0);
      background-color: rgba(0,0,0,0.4);
    }

    .nt-newNote {
      border: 1pt solid #b7b5b5;
      border-radius: 6px;
      background-color: #dddbdb;
      margin:5% auto;
      padding: 10px;
      width:80%;
    }
    .nt-displayNote {
      border: 1pt solid #b7b5b5;
      border-radius: 6px;
      background-color: #dddbdb;
      margin:5% auto;
      padding: 10px;
      width:80%;
      height:80%;
      overflow-y:scroll;
      scrollbar-base-color:burlywood;
    }

    .nt-displayNoteContent {
      font-family:Tahoma;
      font-size:16px;
    }

    .nt-newNote-but {
      display: flex;
      flex-direction: row;
      justify-content: flex-end;
      flex-wrap: wrap;
      margin: 10px;
    }

    .nt-otherNote {
      display: flex;
      flex-direction: row;
      justify-content: flex-start;
    }

    .nt-otherNote-msg {
      display: flex;
      flex-direction: column;
      border: 1pt solid #b7b5b5;
      border-radius: 6px;
      background-color: #dddbdb;
      max-width: 80%;
      padding: 4px;
      margin: 6px;
    }

    .nt-otherNote-msg-h {
      display:flex;
      flex-direction:row;
      justify-content:space-between;
      background-color: royalblue;
      border-top-left-radius: inherit;
      border-top-right-radius: inherit;
      font-family:Tahoma;
      font-size:8px;
      font-weight:bold;
      padding: 5px;
      color: white;
    }

    .nt-otherNote-msg-b {
      background-color: lightyellow;
      border-bottom-left-radius: inherit;
      border-bottom-right-radius: inherit;
      font-size:12px;
      margin: 2px;
      color: black;
    }

    .nt-note-attachment {
      background-color: lightgreen;
      border-bottom-left-radius: inherit;
      border-bottom-right-radius: inherit;
      margin: 0px 0px 0px 0px;
      color: black;
    }

    .nt-note-attachment-link {
      margin: 2px;
      color: black;
      font-family: 'Courier New';
      font-size: 12px;
    }

    .nt-myNote {
      display: flex;
      flex-direction: row;
      justify-content: flex-end;
    }

    .nt-myNote-msg {
      display: flex;
      flex-direction: column;
      border: 1pt solid #b7b5b5;
      border-radius: 6px;
      background-color: #dddbdb;
      max-width: 80%;
      padding: 4px;
      margin: 6px;
    }

    .nt-myNote-msg-h {
      display:flex;
      flex-direction:row;
      justify-content:space-between;
      background-color: #04aeae;
      border-top-left-radius: inherit;
      border-top-right-radius: inherit;
      font-family:Tahoma;
      font-size:8px;
      font-weight:bold;
      padding: 5px;
      color: white;
    }

    .nt-myNote-msg-b {
      background-color: #78e1e1;
      border-bottom-left-radius: inherit;
      border-bottom-right-radius: inherit;
      font-size:12px;
      margin: 2px;
      color: black;
    }
    .nt-sendto {
      color:crimson;
      font-size:12px;
      cursor:pointer;
    }
    .nt-icon{
      cursor:pointer;
    }
    .nt-icon:hover{
      color:gold;
    }
    .nt-div-row{
      margin:2px;
    }
    .nt-thumb{
      height:102px;
      width:100%;
      padding:1px;
      background-color:black;
      text-align:center;
      vertical-align:middle;
    }
    .nt-fullimg{
      height:100%;
      width:100%;
      padding:1px;
      background-color:black;
      text-align:center;
      vertical-align:middle;
    }

    .nt-selectedfile-div{
      display:flex;
      flex-direction:column;
      justify-content:flex-start;
    }
    .nt-selectedfile-list{
      margin:2px;
      padding:2px;
      color:blue;
      font-family:Tahoma;
      font-size:10px;
    }
    .nt-err-msg{
      color:red;
      font-weight:bold;
      font-family:Tahoma;
      font-size:12px;
    }
    .nt-close {
      color: #ff6a00;
      float: right;
      font-size: 44px;
      font-weight: bold;
    }
    .nt-close:hover,
    .nt-close:focus {
      color: red;
      text-decoration: none;
      cursor: pointer;
    }
  </style>
  <script>
    function $get(str) {
      return document.getElementById(str);
    }
    var nt_script = {
      newShown: false,
      show_new_server: function () {
        $get('newNote').style.display = 'block';
        this.setKeyWords();
        return false;
      },
      show_new: function (o) {
        $get('F_ChildNotesID').value = '';
        if (typeof (o) != 'undefined') {
          var xStr = o.innerText.trim();
          var iStr = o.dataset.toinclude;
          if (iStr != '') {
            if (xStr != '') {
              xStr = xStr + ',' + iStr;
            } else {
              xStr = iStr;
            }
          }
          $get('txtMailTo').value = xStr;
          $get('F_ChildNotesID').value = o.dataset.notesid;
          this.setKeyWords();
        }
        $get('newNote').style.display = 'block';
        return false;
      },
      hide_new:function(){
        $get('newNote').style.display = 'none';
        return false;
      },
      display_img: function (z) {
        var t = $get('displayNoteContent');
        t.innerHTML = '';
        if (typeof (z) != 'undefined') {
          try {
            t.innerHTML = '<div class=\'nt-fullimg\'><img src=\'' + z.src + '\'/></div>';
          } catch (e) { }
        }
        $get('displayNote').style.display = 'block';
        return false;
      },

      display_note: function (z) {
        var t = $get('displayNoteContent');
       t.innerHTML='';
       if (typeof (z) != 'undefined') {
         t.innerHTML = '<p>'+ $get(z.replace('bs', 'title')).innerHTML + '</p><br/>';
         t.innerHTML = t.innerHTML + $get(z.replace('bs', 'lp')).innerHTML;
         try{
           t.innerHTML = t.innerHTML + $get(z.replace('bs', 'mp')).innerHTML;
         }catch(e){}
        }
        $get('displayNote').style.display = 'block';
        return false;
      },
      hide_display:function(){
        $get('displayNote').style.display = 'none';
        return false;
      },
      show_read: function (o) {
        var id = o.id.split('_')[1];
        var dots = $get('dots_'+id);
        var moreText = $get('more_'+id);
        var btnText = $get('cmd_'+id);

        if (dots.style.display === "none") {
          dots.style.display = "inline";
          btnText.innerHTML = "Read more";
          moreText.style.display = "none";
        } else {
          dots.style.display = "none";
          btnText.innerHTML = "Read less";
          moreText.style.display = "inline";
        }
        return false;
      },
      download: function (o) {
        var cmd = $get('cmdDownload');
        $get('cmdText').value = o.dataset.drid;
        $get('cmdDownload').click();
        return true;
      },
      choose_file:function() {
        $get('f_Uploads').click();
        return false;
      },
      CTRLUpload:'',
      filesSelected: function (evt) {
        this.CTRLUpload = evt.target;
        var files = evt.target.files; // FileList object
        var output = [];
        for (var i = 0, f; f = files[i]; i++) {
          var n = f.name.replace(/,/g, '-').replace(/'/g, '-');
          output.push(
             '<div class="nt-selectedfile-list">', n, '</div>'
          );
        }
        $get('UploadFileList').innerHTML = output.join('');
      },
      add_err:function(x){
        var de = $get('divErr');
        de.innerHTML = de.innerHTML + '<li class="nt-err-msg">' + x + '</li>';
      },
      clear_err: function () {
        var de = $get('divErr');
        de.innerHTML = '';
        de.style.display = 'none';
      },
      show_err: function () {
        var de = $get('divErr');
        de.style.display = 'block';
      },
      validate_note: function () {
        this.clear_err();
        var err = false;
        var title = $get('txtTitle');
        var desc = $get('txtDescription');
        var smail = $get('txtMailTo');
        var rmail = $get('txtReminderTo');
        var rdate = $get('txtDate');
        var keyWd = $get('dmsKeyWordInput');
        var atchs = 0;
        if (keyWd.value != '') {
          this.addToEMailIDs(keyWd);
        }
        $get('txtMailTo').value = nt_script.getKeyWords($('.dms-word-value'));
        if (typeof(this.CTRLUpload) === 'object') {
          atchs = this.CTRLUpload.files.length;
        }
        if (atchs <= 0) {
          if (title.value == '' && desc.value == '') {
            this.add_err('Title & Description or Attachment is required.');
            err = true;
          }
        }
        if(rmail.value!='' && rdate.value==''){
          this.add_err('Reminder date required.');
          err = true;
        }
        if (err) {
          this.show_err();
          return false;
        }
        return true;
      },
      submit_note: function () {
        return this.validate_note();
      },
      setKeyWords: function () {
        var fv = $get('txtMailTo').value.split(',');
        if (fv.length > 0) {
          var dv = $get('divWords');
          dv.innerHTML = '';
          for (var i = 0; i < fv.length; i++) {
            dv.innerHTML += this.DmsWord(fv[i]);
          }
        }
      },
      getKeyWords: function (x) {
        var y = '';
        for (i = 0; i < x.length; i++) {
          if (y == '')
            y = x[i].innerText;
          else
            y = y + ',' + x[i].innerText;
        }
        return y;
      },
      DmsWord: function (x) {
        var output = [];
        output.push(
          '<div class="dms-word">',
            '<div class="dms-word-value">',
               x,
            '</div>',
            '<div class="dms-word-remove" onclick="$(this).parent().fadeOut(300) && nt_script.removeKey(this);">&times;</div>',
          '</div>'
          );
        return output.join('');
      },
      removeKey: function (x) {
        x.parentNode.parentNode.removeChild(x.parentNode);
      },
      dmsKey: function (e) {
        var y = e.char || e.key;
        if (y == ',') {
          var t = e.target;
          this.addToEMailIDs(t);
          e.preventDefault();
          return false;
        }
      },
      addToEMailIDs:function(t){
        var aEMails = this.getEMailIDs(t.value);
        for (var i = 0; i < aEMails.length;i++){
          var z = this.DmsWord(aEMails[i]);
          $get('divWords').innerHTML += z;
        }
        t.value = '';
        return;
      },
      getEMailIDs: function (str) {
        var ret = [];
        var tVal = str.trim();
        var IDs = tVal.split(',');
        for (var i = 0; i < IDs.length; i++) {
          var xIDs = IDs[i].trim().split(';');
          for (var j = 0; j < xIDs.length; j++) {
            var jID = xIDs[j].trim();
            if (jID != '') {
              ret.push(jID);
            }
          }
        }
        return ret;
      },
      dmsHelpNode: function (x) {
        var output = [];
        output.push(
          '<div class="dms-popup-value" onclick="nt_script.dmsHelpNodeSelected(this);">',
               x,
          '</div>'
          );
        return output.join('');
      },
      dmsHelpNodeSelected: function (x) {
        var ib = $get('dmsKeyWordInput');
        var p = x.innerHTML.split(':');
        if (p.length == 3) {
          if (p[2] != '') {
            $get('divWords').innerHTML += this.DmsWord(p[2]);
          }
        }
        ib.value = '';
        ib.focus();
      },
      dmsKeyWordHelp: function (event) {
        var e = event;
        var tgt = e.target;
        var str = tgt.value;
        var pc = $get('dmsPopup');
        if (event.char == ',') {
          return;
        }
        if (event.keyCode == 27) {
          pc.style.display = 'none';
        }
        pc.style.top = parseFloat(tgt.style.top) + parseFloat(tgt.style.height) + 25 + 'px';
        pc.style.left = parseFloat(tgt.style.left) + 'px';
        pc.style.width = (parseFloat(tgt.style.width) || tgt.clientWidth) + 'px';
        pc.style.display = 'flex';
        
        document.addEventListener("click", function (event) {
          $get('dmsPopup').style.display = 'none';
        });
        window.event.returnValue = false;
        pc.innerHTML = '';
        var that = nt_script;
        $.ajax({
          type: 'POST',
          url: '/Attachment/App_Services/ntHandler.asmx/getEmailIDs',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + str + "',cnt:'" + 10 + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            var pc = $get('dmsPopup');
            for (var i = 0, f; f = y.strHTML[i]; i++) {
              pc.innerHTML += nt_script.dmsHelpNode(f);
            }
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      }
    }
  </script>
  <style>
    /*keygen Word*/
    .dms-word-container {
      display: flex;
      flex-wrap: wrap;
      background-color:#f2f2f2;
      border: 1pt solid gray;
      border-radius: 4px;
      font-family: 'Courier New';
      font-size: 12px;
      min-height:40px;
    }

    .dms-word {
      display: flex;
      flex-direction: row;
      background-color: #e8e3e3;
      border: 1pt solid #9b9898;
      margin: 2px;
      padding: 2px;
      border-radius: 5px;
    }

    .dms-word-remove {
      padding-left: 6px;
      color: #9b9898;
      cursor: pointer;
      vertical-align: top;
    }

      .dms-word-remove:hover {
        color: orangered;
      }

    .dms-popup-container {
      display: none;
      flex-direction: column;
      background-color: #eed54d;
      border: 1pt solid #cbad06;
      position: absolute;
      z-index: 10005;
      font-family: 'Courier New';
      font-size: 12px;
    }

    .dms-popup-value {
      background-color: #fbe778;
      border: 1pt solid #ffd800;
      margin: 1pt;
      cursor: pointer;
    }

      .dms-popup-value:hover {
        background-color: #fff5c0;
      }

  </style>
</head>
<body>
  <form runat="server" id="form1" enctype="multipart/form-data">
    <div class="nt-conainer">
      <%--Header--%>
      <div class="nt-header">
        <div id="divHeader" runat="server">
          
        </div>
        <div>
          <input type="button" class="nt-but-danger" value="New Note" onclick="return nt_script.show_new();" />
        </div>
      </div>
      <%--Body--%>
      <div class="nt-body" style="background-color: aqua;">
        <%--New Note--%>
        <div id="newNote" class="nt-modal-container">
          <div  class="nt-newNote">
            <div style="font-size: 14px; font-weight: bold; margin: 6px;">
            <asp:TextBox ID="F_ChildNotesID" runat="server" style="display:none;" ></asp:TextBox>
               NEW Note:
            </div>
            <div id="divErr" runat="server" style="display:none;padding:10px;text-align:center;">

            </div>
            <div>
              <asp:TextBox ID="txtTitle" runat="server" CssClass="nt-input-box" Height="30px" ValidationGroup="submit" placeholder="Title"></asp:TextBox>
            </div>
            <div style="padding-right:2px;">
              <asp:TextBox ID="txtDescription" runat="server" CssClass="nt-input-box" style="min-height:150px;resize:vertical;" placeholder="Description" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div style="padding-right:2px;">
<%--              <asp:TextBox id="txtMailTo" runat="server" class="nt-input-box" style="min-height:40px;resize:vertical;" placeholder="Send Mail to <email id 1>,<email id 2>..." TextMode="MultiLine"></asp:TextBox>--%>
              <div id="divWords" class="dms-word-container"></div>
              <input id="dmsKeyWordInput" type="text" class="nt-input-box" placeholder="Send Mail to <email id 1>,<email id 2>...[Press comma ',' to complete NEW email ID]" onkeydown="nt_script.dmsKey(event);" />
              <div id="dmsPopup" class="dms-popup-container"></div>
              <asp:TextBox ID="txtMailTo"
                style="display:none;"
                runat="server" />

            </div>
            <div>
           </div>
           <div style="font-size: 14px; font-weight: bold; margin: 6px;">
              Reminder Setting:
            </div>
            <div style="padding-right:2px;">
              <asp:TextBox id="txtReminderTo" runat="server" class="nt-input-box" style="min-height:40px;resize:vertical;" placeholder="Reminder Mail to <email id 1>,<email id 2>..." TextMode="MultiLine"></asp:TextBox>
            </div>
            <div>
              <asp:TextBox id="txtDate" type="date" runat="server" class="nt-input-box" style="height:30px;" placeholder="Reminder Date [DD/MM/YYYY]"/>
            </div>
            <div class="nt-newNote-but">
              <div class="nt-selectedfile-div">
                <div style="color:deeppink;font-family:Tahoma;font-size:10px;font-style:italic;">Selected File(s):</div>
                <div id='UploadFileList'></div>
              </div>
             <div style="display:none;">
                <input type="file" id="f_Uploads" runat="server" name="f_Uploads[]" multiple="multiple" onchange="return nt_script.filesSelected(event);">
              </div>
              <div style="margin:5px;">
                <asp:Button ID="cmdAttach" runat="server" CssClass="nt-but-primary" Text="Attachment" OnClientClick="return nt_script.choose_file();" />
              </div>
              <div style="margin:5px;">
                <asp:Button ID="cmdClose" runat="server" CssClass="nt-but-danger" Text="Close" OnClientClick="return nt_script.hide_new(this);" />
              </div>
              <div style="margin:5px;">
                <asp:Button ID="cmdSubmit" runat="server" CssClass="nt-but-success" Text="Submit" OnClientClick="return nt_script.submit_note();" />
              </div>
            </div>
          </div>

        </div>
        <%--Old Notes List--%>
        <div id="oldNote" runat="server" >
          <input type="button" value="dummy" id="cmdDownload" runat="server" onserverclick="cmdDownload_Click" style="display:none;" />
          <asp:TextBox ID="cmdText" runat="server" style="display:none;"></asp:TextBox>

        </div>
        <%--Display Note--%>
        <div id="displayNote" class="nt-modal-container">
          <div>
          <span class="nt-close" onclick="return nt_script.hide_display();">&times;</span>
          </div>
         <div class="nt-displayNote">
            <div class="nt-displayNoteContent" id="displayNoteContent">
            </div>
          </div>
        </div>

      </div>
    </div>
  </form>
  <div id="afterLoad" runat="server"></div>
  <script>
    //document Ready
    window.history.replaceState('', '', window.location.href);
    $get('dmsKeyWordInput').addEventListener('keyup', nt_script.dmsKeyWordHelp);

  </script>
</body>
</html>
