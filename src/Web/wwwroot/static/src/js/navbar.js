// navbar-search
// Muestra el input search
$('.navbar-search-button').on('click', function () {
  $('.navbar-search').removeClass('hidden')
  $('.navbar-search-input').focus()
})

$('.navbar-search-close').on('click', function () {
  $('.navbar-search').addClass('hidden')
})

$('.navbar-search-input').keyup(function (e) {
  if (e.keyCode === 27) {
    $('.navbar-search').addClass('hidden')
  }
})

/**
 * En lg siempre esta fixed-top
 * En md o menor el fixed se elimina
 */
const initializeWindowSize = function () {
  const navbar = $('#main-navbar')
  const body = $('body')

  function fixedTop() {
    navbar.removeClass('fixed-top')
    body.css('padding-top', '0')
  }

  function unFixedTop() {
    navbar.addClass('fixed-top')
    body.css('padding-top', '60px')
  }

  function checkWindowSize() {
    const widthSize = $(window).width()

    if (widthSize < 960) {
      fixedTop()
    } else {
      unFixedTop()
    }
  }

  window.onresize = () => {
    checkWindowSize()
  }

  checkWindowSize()
}

initializeWindowSize()
