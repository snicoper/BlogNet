/* global hljs */

// Bootstrap tooltip
$(document).ready(function () {
  $('[data-toggle="tooltip"]').tooltip()

  // https://highlightjs.org/usage/
  $('pre code').each(function (i, block) {
    hljs.highlightBlock(block)
  })

  /**
   * Muestra el nombre del archivo en input.custom-file
   */
  $('.custom-file-input').on('change', function () {
    let fileName = $(this).val().split('\\').pop()
    $(this).next('.custom-file-label').addClass('selected').html(fileName)
  })
})

// toastr
toastr.options.closeButton = true

// // csrf axios
// axios.defaults.xsrfCookieName = 'csrftoken'
// axios.defaults.xsrfHeaderName = 'X-CSRFToken'

/**
 * Cortar un string a length caracteres
 */
function truncateText(text, length) {
  if (text) {
    const dots = text.length >= length ? '...' : ''
    return text.substring(0, length) + dots
  }
  return ''
}

/**
 * Remplaza \n por <br>
 */
function lineBreakBr(text) {
  return text.replace(/\n/g, '<br>')
}
