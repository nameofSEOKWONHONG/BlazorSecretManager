window.initializeDraggablePopup = (popupId) => {
    let popup = document.getElementById(popupId);
    let header = document.getElementById(popupId + "-header");

    header.onmousedown = function (e) {
        startDrag(e, popup);
    };
};

function startDrag(e, popup) {
    e.preventDefault();
    let pos1 = 0, pos2 = 0, pos3 = e.clientX, pos4 = e.clientY;

    document.onmousemove = function (event) {
        event.preventDefault();
        pos1 = pos3 - event.clientX;
        pos2 = pos4 - event.clientY;
        pos3 = event.clientX;
        pos4 = event.clientY;
        popup.style.top = (popup.offsetTop - pos2) + "px";
        popup.style.left = (popup.offsetLeft - pos1) + "px";
    };

    document.onmouseup = function () {
        document.onmousemove = null;
        document.onmouseup = null;
    };
}

// 🎯 리사이즈 기능 추가
window.initializeResizablePopup = (popupId) => {
    let popup = document.getElementById(popupId);
    let resizer = document.getElementById(popupId + "-resizer");

    resizer.onmousedown = function (e) {
        startResize(e, popup);
    };
};

function startResize(e, popup) {
    e.preventDefault();
    let startX = e.clientX;
    let startY = e.clientY;
    let startWidth = popup.offsetWidth;
    let startHeight = popup.offsetHeight;

    document.onmousemove = function (event) {
        event.preventDefault();
        let newWidth = startWidth + (event.clientX - startX);
        let newHeight = startHeight + (event.clientY - startY);

        if (newWidth > 200) popup.style.width = newWidth + "px";
        if (newHeight > 150) popup.style.height = newHeight + "px";
    };

    document.onmouseup = function () {
        document.onmousemove = null;
        document.onmouseup = null;
    };
}

window.preventEnterDefault = (dotNetHelper, element) => {
    element.addEventListener("keydown", function (event) {
        if (event.key === "Enter" && !event.shiftKey) {
            event.preventDefault(); // 기본 줄바꿈 방지
            //dotNetHelper.invokeMethodAsync("OnEnterPressed"); // Blazor에서 OnEnterPressed 호출
        }
    });
};

window.observeScrollTop = (dotNetHelper) => {
    let chatContainer = document.getElementById("chat-container");
    let scrollTopDetector = document.getElementById("scroll-top");

    let observer = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting) {
            dotNetHelper.invokeMethodAsync("OnScrollTopReached");
        }
    }, { root: chatContainer, threshold: 0.1 });

    observer.observe(scrollTopDetector);
};

window.maintainScrollPosition = (element) => {
    let oldHeight = element.scrollHeight;
    setTimeout(() => {
        element.scrollTop = element.scrollHeight - oldHeight;
    }, 0);
};

window.scrollDownBy = (pixels) => {
    let chatContainer = document.getElementById("chat-container");
    if (chatContainer) {
        chatContainer.scrollTop += pixels;
    }
};