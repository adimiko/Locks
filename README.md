<div align="center">
    <img src="assets/locks-rounded-social-logo.png" >
</div>

<h3 align="center">Synchronization mechanisms in .NET 🔐</h3>

<h4 align="center"><i>Prevent race contidion</i></h3>

<p align="center">
  <a href="https://www.nuget.org/packages/Locks"><strong>Package</strong></a>
  
</p>
<div align="center">
  
  [![Build & Tests](https://github.com/adimiko/Locks/actions/workflows/build-and-tests.yaml/badge.svg?branch=main)](https://github.com/adimiko/Locks/actions/workflows/build-and-tests.yaml)

##### :star: - The star motivates me a lot!   

</div>


### Usage
#### MemoryLock
```C#
using (await memoryLock.AcquireAsync("YOUR_KEY"))
{
   // Shared resource (multi-threaded environment)
}
```

#### DistributedLock
```C#
await using (await distributedLock.AcquireAsync("YOUR_KEY"))
{
   // Shared resource (distributed environment)
}
```

## :balance_scale: License
This project is under the [MIT License](https://github.com/adimiko/Locks/blob/main/LICENSE).

## :radioactive: Disclaimer
The project is under development and not ready for production use.
