Calling Yarn manually:

    yarn run rollup -c
    

Calling it in csproj:

        <Target Name="GenerateSomeFiles" BeforeTargets="BeforeBuild">
            <Exec Command="yarn run rollup -c" />
        </Target>