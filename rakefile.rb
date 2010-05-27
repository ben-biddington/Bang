require 'bluecloth'
require 'maruku'
require 'date'

desc 'Preview readme as html'
task 'preview_readme' do |task|
	puts Maruku.new(File.read('readme.markdown')).to_html_document
end

require 'albacore'

desc "Clean and build"
msbuild 'clean_and_build' do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.verbosity = "quiet"
    msb.solution  = "Bang.sln"
end

desc "Run integration tests"
nunit :integration_test do |nunit|
	working_dir = File.dirname(__FILE__)
	out_dir = "#{working_dir}/out"
	release_or_debug = 'Debug'
	
	Dir.mkdir(out_dir) unless File.exists?(out_dir)
	
	puts "Working directory: #{working_dir}"
	puts "Output directory: #{out_dir}"
	
	nunit.path_to_command = "#{working_dir}/bin/nunit-console.exe"
	nunit.assemblies "#{working_dir}/test/Bang.Integration.Tests/bin/#{release_or_debug}/Bang.Integration.Tests.dll"
	
	# See: http://www.nunit.org/index.php/docs/docs/2.5.2/files/index.php?p=consoleCommandLine&r=2.4
	nunit.options "/xml=#{out_dir}/tests-results.xml", '/nologo'
end
