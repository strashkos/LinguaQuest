window.linguaQuest = {
  highlight(selector) {
    const nodes = document.querySelectorAll(selector);
    nodes.forEach((node) => {
      node.classList.remove("pulse");
      void node.offsetWidth;
      node.classList.add("pulse");
    });
  }
};