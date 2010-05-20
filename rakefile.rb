require 'bluecloth'
require 'maruku'
require 'date'

desc 'Preview readme as html'
task 'preview_readme' do |task|
	puts Maruku.new(File.read('readme.markdown')).to_html_document
end