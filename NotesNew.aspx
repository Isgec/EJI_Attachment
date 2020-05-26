<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NotesNew.aspx.vb" Inherits="NotesNew" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
  <title>ISGEC-Notes</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link href="Res/fa/css/all.css" rel="stylesheet" />
  <style>
    html, body {
      height: 100%;
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
      display: flex;
      flex-direction: column;
      margin: 3px !important;
      border-radius: 6px;
      background-color: white;
      height: 97%;
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

    .nt-newNote {
      display: none;
      border: 1pt solid #b7b5b5;
      border-radius: 6px;
      background-color: #dddbdb;
      margin: 2px;
      padding: 4px;
    }

    .nt-newNote-but {
      display: flex;
      flex-direction: row;
      justify-content: flex-end;
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
      padding: 2px;
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
      padding: 2px;
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
    .nt-icon{
      cursor:pointer;
    }
    .nt-icon:hover{
      color:gold;
    }
  </style>
  <script>
    var nt_script = {
      newShown: false,
      show_new: function (o) {
        var x = document.getElementById('newNote');
        if (o.value == 'New Note') {
          x.style.display = 'block';
          o.value = 'Hide';
          this.newShown = true;
        } else {
          x.style.display = 'none';
          o.value = 'New Note';
          this.newShown = false;
        }
        return true;
      },
      show_read: function (o) {
        var id = o.id.split('_')[1];
        var dots = document.getElementById('dots_'+id);
        var moreText = document.getElementById('more_'+id);
        var btnText = document.getElementById('cmd_'+id);

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
      }
    }
  </script>
</head>
<body>
  <form runat="server" id="form1">
    <div class="nt-conainer">
      <%--Header--%>
      <div class="nt-header">
        <div>
          abcd
        </div>
        <div>
          <input type="button" class="nt-but-danger" value="New Note" onclick="return nt_script.show_new(this);" />
        </div>
      </div>
      <%--Body--%>
      <div class="nt-body" style="background-color: aqua;">
        <%--New Note--%>
        <div id="newNote" class="nt-newNote">
          <div style="font-size: 14px; font-weight: bold; margin: 6px;">
            NEW Note:
          </div>
          <div>
            <input type="text" class="nt-input-box" style="height: 30px;" placeholder="* Title" required="required" />
          </div>
          <div>
            <textarea class="nt-input-box" style="min-height: 50px;" placeholder="* Description" required="required" aria-required="true"></textarea>
          </div>
          <div>
            <textarea class="nt-input-box" style="min-height: 50px;" placeholder="Send Mail to <email id 1>,<email id 2>..."></textarea>
          </div>
          <div class="nt-newNote-but">
            <div>
              <asp:Button ID="cmdReset" runat="server" CssClass="nt-but-danger" Text="Reset" />
            </div>
            <div style="margin-left: 10px;">
              <asp:Button ID="cmdSubmit" runat="server" CssClass="nt-but-success" Text="Submit" />
            </div>
          </div>
        </div>
        <%--Old Notes List--%>
        <div id="oldNote">

          <div class="nt-otherNote">
            <div class="nt-otherNote-msg">
              <div class="nt-otherNote-msg-h">
                <div>header</div>
                <div class="nt-icon"><i class="fas fa-angle-down"></i></div>
              </div>
              <div class="nt-otherNote-msg-b">
                <p>
                    Title of content fksdjfkjfdk sdfkjskdjk
                </p>
                body of contet
                jsdkjkjkj dkkjsdk ldfkgldfkgl dfg;fdkfdk fdmlblfgmbb dmdkfdfkn dfmndfnn
                <div class="nt-note-attachment">
                  <div class="nt-note-attachment-link">
                    <a href="#">abc.docx</a>
                  </div>
                </div>
              </div>

            </div>
          </div>

          <div class="nt-myNote">
            <div class="nt-myNote-msg">
              <div class="nt-myNote-msg-h">
                <div>sakkfhfsj fskjksjf fskskf</div>
                <div class="nt-icon"><i class="fas fa-angle-down"></i></div>
              </div>
              <div class="nt-myNote-msg-b">
                <p>
                    Title of content fksdjfkjfdk sdfkjskdjk
                </p>
                jsdkjkjkj dkkjsdk ldfkgldfkgl dfg;fdkfdk fdmlblfgmbb dmdkfdfkn dfmndfnn fdllkg
                shfjdhfjd<span id="dots_596">...</span><span id="more_596" style="display:none;">more content kd sddkksdhf sdfkfjks sdfjksjd
                  fhsjfhjsdh fnksfdksdn sdksdjk sdfksjfks sdjfsdjfsjd fdsjksjfkjsd fjskjfks fjsdfjks flsjdfjs</span>
                <button class="nt-but-grey" onclick="return nt_script.show_read(this);" id="cmd_596">Read more</button>
                <div class="nt-note-attachment">
                  <div class="nt-note-attachment-link">
                    <a href="#">xyz.docx</a>
                  </div>
                </div>
              </div>

            </div>
          </div>

        </div>
      </div>
    </div>
  </form>
</body>
</html>
