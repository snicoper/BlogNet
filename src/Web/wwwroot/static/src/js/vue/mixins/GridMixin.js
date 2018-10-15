/* global truncateText, GridPaginate */

const gridMixin = {
  data() {
    return {
      loading: false,
      datePickerOptions: null,
      objectList: [],

      // Ordenación
      sortOrder: null,
      orderBy: null,

      // Paginación
      totalItems: null,
      pageNumber: null,
      pageSize: '',
      totalPages: 1
    }
  },

  mounted() {
    this.datePickerOptions = {
      language: 'es',
      format: 'dd/mm/yyyy',
      clearBtn: true
    }
  },

  created() {
    this._getObjectList()
  },

  watch: {
    pageSize(val) {
      this.pageSize = val
      this._onFilterChange()
    }
  },

  methods: {
    truncateText(text, length) {
      return truncateText(text, length)
    },

    onChangePagination(page) {
      this.pageNumber = page
      this._getObjectList()
    },

    onOrderBy(field) {
      this.orderBy = field
      this.sortOrder = this.sortOrder === 1 ? 2 : 1
      this._getObjectList()
    },

    onBtnFilterClick() {
      const btn = $('#icon-filter')

      if (btn.hasClass('fa-caret-down')) {
        btn.removeClass('fa-caret-down')
        btn.addClass('fa-caret-up')
      } else {
        btn.removeClass('fa-caret-up')
        btn.addClass('fa-caret-down')
      }
    },

    getCaretOrder(field) {
      if (this.orderBy === field) {
        if (this.sortOrder === 1) {
          return '<i class="fas fa-caret-down"></i>'
        } else {
          return '<i class="fas fa-caret-up"></i>'
        }
      }
      return ''
    },

    findElementById(id) {
      for (let i = 0; i < this.objectList.length; i++) {
        if (this.objectList[i].id === id) {
          return {
            index: i,
            element: this.objectList[i]
          }
        }
      }
    },

    _onDelete(url, id) {
      const element = this.findElementById(id)

      if (element.index !== null) {
        // eslint-disable-next-line
        if (confirm("¿Seguro que lo quieres eliminar?") === true) {
          axios({
            method: 'delete',
            url: url
          })
            .then((response) => {
              if (response.status === 204) {
                this.objectList.splice(element.index, 1)
                this.totalItems--
                toastr.success('Eliminado con éxito')
              }
            })
        }
      } else {
        toastr.error('Elemento no encontrado')
      }
    },

    _onFilterChange() {
      // Reset pagination
      this.$refs.paginate.selected = 0
      this.pageNumber = 1
      this._getObjectList()
    }
  },

  computed: {
    getObjectList() {
      return this.objectList !== null && !this.loading ? this.objectList : []
    },

    isEmptyAndNotLoading() {
      return !this.loading && this.objectList && !this.objectList.length
    }
  }
}
