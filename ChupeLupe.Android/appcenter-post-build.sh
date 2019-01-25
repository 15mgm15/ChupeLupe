#!/usr/bin/env bash
#
# For Xamarin, run all NUnit test projects that have "Test" in the name.
# The script will build, run and display the results in the build logs.
APP_TEST_PROJECT=$APPCENTER_SOURCE_DIRECTORY/ChupeLupe/UnitTest/UnitTest.csproj
APP_TEST_DLL=$APPCENTER_SOURCE_DIRECTORY/ChupeLupe/UnitTest

echo "------------ Starting Unit Test Process ------------"

echo "Found NUnit test projects:"
find APP_TEST_PROJECT -exec echo {} \;
echo 
echo "Building NUnit test projects:"
find APP_TEST_PROJECT -exec msbuild {} \;
echo
echo "Compiled projects to run NUnit tests:"
find APP_TEST_DLL  -regex '.*bin.*UnitTest.dll' -exec echo {} \;
echo
echo "Running NUnit tests:"
find APP_TEST_DLL -regex '.*bin.*UnitTest.dll' -exec nunit3-console {} +
echo
echo "NUnit tests result:"
pathOfTestResults=$(find $APPCENTER_SOURCE_DIRECTORY -name 'TestResult.xml')
cat $pathOfTestResults
echo

#look for a failing test
grep -q 'result="Failed"' $pathOfTestResults

if [[ $? -eq 0 ]]
then 
echo "------------ A test Failed!!! --------------" 
exit 1
else 
echo "------------ All tests passed :D --------------"
fi
