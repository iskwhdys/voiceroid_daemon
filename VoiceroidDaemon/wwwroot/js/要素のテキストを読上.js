javascript:
{
    let hoverStyle = document.createElement("style");
    hoverStyle.innerHTML = "*:hover { outline: 2px solid red !important; }";

    attachDocument();


    function bodyKeyDown_RemoveEvent(event) {
        if (event.keyCode === 27) {
            detachDocumentEvent();
        }
    }

    function attachDocument() {
        document.addEventListener("click", clickElement);
        document.addEventListener("keydown", bodyKeyDown_RemoveEvent);

        document.head.appendChild(hoverStyle);
    }

    function detachDocumentEvent() {
        document.removeEventListener("click", clickElement);
        document.removeEventListener("keydown", bodyKeyDown_RemoveEvent);

        document.head.removeChild(hoverStyle);
    }



    function clickElement(event) {
        var clickedElement = event.target;

        detachDocumentEvent();

        doPost(clickedElement.outerText);
    }

    function doPost(callText) {
        console.log(callText);
        var options = {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'text/plain; charset=utf-8'
            },
            body: callText
        };

        fetch('http://localhost:8080/play', options)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.body;
            })
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                alert("Error:" + error);
            });
    }
}