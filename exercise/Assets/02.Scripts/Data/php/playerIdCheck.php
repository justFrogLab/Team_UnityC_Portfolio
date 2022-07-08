<?php
$hostname = 'localhost';
$username = 'root';
$password = '0000';
$database = 'playerDataBase';
$secrtKey = "UnityC";

// data base 접속
try {	
	$dbh = new PDO('mysql:host='.$hostname.';dbname='.$database, $username, $password);
}

catch(PDOException $e){
	echo '<h1> An error has occureed. </h1><pre>', $e ->getMessage(), '</pre>';
}

$hash = $_GET['hash'];
$realHash =hash('sha256',$_GET['id'].$secrtKey);

$playerId =  $_GET['id'];

$resultId;
// query 문
$sthId = $dbh->query("SELECT * FROM playerlogin WHERE id LIKE '$playerId'");

// Id 검출
try
{	
		// excute 성공
	if($sthId -> execute()) { echo "";}
	else{echo "excute 실패 ㅠㅠㅠ!!!";}


	$sthId -> setFetchMode(PDO::FETCH_ASSOC);
	$resultId = $sthId->fetchALL();

	if(count($resultId) >0)
	{
		foreach($resultId as $r)
		{
			echo "exist";
		}
	}

	else
	{
		echo "null";
	}

}

// pw 검출
catch(Exception $e)
{
	echo'<h1>An error has ocurred</h1><pre>',
	$e-> getMessage(),'</pre>';
}
